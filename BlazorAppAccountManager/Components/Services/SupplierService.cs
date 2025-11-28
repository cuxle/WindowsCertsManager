using BlazorAppAccountManager.Components.Data;
using BlazorAppAccountManager.Components.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorAppAccountManager.Components.Services
{
    public class SupplierService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;
        private readonly ILogger<SupplierService> _logger;

        // 构造函数注入依赖
        public SupplierService(IDbContextFactory<ApplicationDbContext> dbFactory, ILogger<SupplierService> logger)
        {
            _dbFactory = dbFactory;
            _logger = logger;
        }

        // 处理引用回传逻辑（CertificateRecord级别）
        public async Task<(bool IsSuccess, string ErrorMessage)> HandleReferenceBackAsync(CertificateType type, string targetLampId, string sourceSupplierId)
        {
            using var db = await _dbFactory.CreateDbContextAsync();

            // 1. 查询源供应商（被引用的供应商）
            var sourceSupplier = await db.Suppliers
                .FirstOrDefaultAsync(s => s.Id == sourceSupplierId);

            if (sourceSupplier == null)
                return (false, "未找到源供应商");

            // 2. 查询源供应商所属的灯具（获取灯具名和项目名）
            var sourceLamp = await db.Lamps
                .FirstOrDefaultAsync(l => l.Id == sourceSupplier.LampId); // 源供应商的LampId
            if (sourceLamp == null)
                return (false, "未找到源供应商所属的灯具");

            // 3. 查询目标灯具（要引用到的灯具）
            var sourceProject = await db.Projects.FirstOrDefaultAsync(p => p.Id == sourceLamp.ProjectId);
            if (sourceProject == null)
                return (false, "未找到源供应商所属的项目"); 

            var targetLamp = await db.Lamps
                .FirstOrDefaultAsync(l => l.Id == targetLampId);
            if (targetLamp == null)
                return (false, "未找到目标灯具");

            // 4. 验证灯具名称是否相同
            if (sourceLamp.Name != targetLamp.Name)
                return (false, "只能引用相同名称的灯具证书");

            // 若目标灯具下尚无供应商，则自动创建一个默认供应商以便建立引用关系
            string supplierName = $"引用{sourceProject.Name}--{targetLamp.Name}--{sourceSupplier.Name}";
            var targetSupplier = await db.Suppliers
                .FirstOrDefaultAsync(s => s.LampId == targetLampId && s.Name == supplierName);

            if (targetSupplier == null)
            {
                targetSupplier = new Supplier
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = supplierName,
                    LampId = targetLampId,
                    UsingReference = true
                };
                db.Suppliers.Add(targetSupplier);
            }
            
            // 获取源供应商的证书记录
            var sourceCertificateRecord = await db.CertificateRecords
                .FirstOrDefaultAsync(cr => cr.SupplierId == sourceSupplierId && cr.CertificateType == type);

            if (sourceCertificateRecord == null)
                return (false, "源供应商没有证书记录，无法创建引用关系");

            // 创建targetCertificateRecord
            var targetCertificateRecord = new CertificateRecord
            {
                Id = Guid.NewGuid().ToString(),
                SupplierId = targetSupplier.Id,
                CertificateType = sourceCertificateRecord.CertificateType,
                LatestCertificateId = sourceCertificateRecord.LatestCertificateId,
                ReferenceCertificateRecordId = sourceCertificateRecord.Id, // 引用源供应商的证书记录
                UsingReference = true
            };          
            targetSupplier.CertificateRecords.Add(targetCertificateRecord);
            db.CertificateRecords.Add(targetCertificateRecord);
            
            await db.SaveChangesAsync();
            return (true, null);
        }

        // 获取灯具及关联供应商（支持证书记录级别的引用）
        public async Task<(Lamp lamp, List<Supplier> Suppliers)> GetSuppliersByLampIdAsync(string lampId, CertificateType certificateType)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var lamp = await db.Lamps
                .Include(l => l.Suppliers)
                .FirstOrDefaultAsync(l => l.Id == lampId);

            if (lamp == null) return (null, new List<Supplier>());

            // 处理引用状态和加载证书信息
            foreach (var supplier in lamp.Suppliers)
            {
                // 加载供应商的证书记录，包括引用关系
                await db.Entry(supplier)
                    .Collection(s => s.CertificateRecords)
                    .Query()
                    .Include(cr => cr.Certificates)
                    .Include(cr => cr.ReferenceCertificateRecord)
                        .ThenInclude(rcr => rcr.Certificates)
                    .LoadAsync();
                
                // 处理每个证书记录的证书集合（根据引用关系）
                foreach (var record in supplier.CertificateRecords)
                {
                    // 根据引用关系获取正确的证书集合
                    var certificates = GetCertificateRecordCertificates(record);
                    
                    // 创建新的集合以避免修改数据库上下文
                    record.Certificates = new List<Certificate>(certificates);
                }
                
                // 加载当前证书（支持引用证书记录）
                await LoadSupplierCurrentCertificate(db, supplier, lampId, certificateType);
            }

            return (lamp, lamp.Suppliers.ToList());
        }

        public async Task<(bool IsSuccess, string ErrorMessage)> EditSupplierName(string supplierId, string newName)
        {
            if (string.IsNullOrEmpty(supplierId))
                return (false, "supplierId不能为空");

            if (string.IsNullOrEmpty(newName))
                return (false, "supplier Name不能为空");

            using var db = await _dbFactory.CreateDbContextAsync();
            var trackedSupplier = await db.Suppliers
                .FirstOrDefaultAsync(s => s.Id == supplierId);

            if (trackedSupplier == null)
            {
                return (false, $"数据库中未找到ID为 {supplierId} 的供应商");
            }

            // 检查同灯具下是否已有同名供应商（避免重复）
            var duplicateSupplier = await db.Suppliers
                .FirstOrDefaultAsync(s =>
                    s.Name == newName &&
                    s.LampId == trackedSupplier.LampId &&
                    s.Id != trackedSupplier.Id);

            if (duplicateSupplier != null)
            {
                return (false, $"同一灯具下已存在名为 '{newName}' 的供应商");
            }

            // 更新供应商名称
            trackedSupplier.Name = newName;
            db.Suppliers.Update(trackedSupplier);
            await db.SaveChangesAsync();
            return (true, null);
        }

        // 添加供应商
        public async Task<(bool Success, string ErrorMessage)> AddSupplierAsync(string lampId, string name)
        {
            // 基础参数校验
            if (string.IsNullOrWhiteSpace(lampId))
                return (false, "灯具ID不能为空");
            if (string.IsNullOrEmpty(name))
                return (false, "新供应商名称不能为空");

            try
            {
                using var db = await _dbFactory.CreateDbContextAsync();
                var lamp = await db.Lamps
                            .Include(l=>l.Suppliers)
                            .FirstOrDefaultAsync(l => l.Id == lampId);
                if (lamp == null)
                    return (false, $"未找到ID为 {lampId} 的灯具");

                if (await db.Suppliers.AnyAsync(s => s.Name == name && s.LampId == lampId))
                    return (false, $"供应商 '{name}' 已存在于当前灯具");

                var newSupplier = new Supplier
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = name,
                    LampId = lampId,
                    Lamp = lamp,
                    CertificateRecords = new List<CertificateRecord>()
                };

                db.Suppliers.Add(newSupplier);
                lamp.Suppliers.Add(newSupplier);
                await db.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "添加供应商失败");
                return (false, $"添加失败：{ex.Message}");
            }
        }

        // 封装删除供应商逻辑（含关联数据处理）
        public async Task<bool> DeleteSupplierAsync(string supplierId)
        {
            try
            {
                using var db = await _dbFactory.CreateDbContextAsync();
                var supplierToDelete = await db.Suppliers
                    .Include(s => s.CertificateRecords)
                    .FirstOrDefaultAsync(s => s.Id == supplierId);

                if (supplierToDelete == null)
                {
                    _logger.LogInformation($"未找到ID为 {supplierId} 的供应商");
                    return false;
                }

                // 获取要删除的证书记录ID
                var certificateRecordIdsToDelete = supplierToDelete.CertificateRecords?
                    .Select(cr => cr.Id)
                    .ToList() ?? new List<string>();

                // 更新引用这些证书记录的记录
                await db.CertificateRecords
                    .Where(cr => certificateRecordIdsToDelete.Contains(cr.ReferenceCertificateRecordId))
                    .ForEachAsync(cr =>
                    {
                        cr.UsingReference = false;
                        cr.ReferenceCertificateRecordId = null;
                    });

                // 删除关联的证书记录
                if (supplierToDelete.CertificateRecords?.Any() == true)
                    db.CertificateRecords.RemoveRange(supplierToDelete.CertificateRecords);

                // 删除供应商本身
                db.Suppliers.Remove(supplierToDelete);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"删除供应商 {supplierId} 失败");
                return false;
            }
        }

        // 更新供应商
        public async Task<bool> UpdateSupplierAsync(Supplier supplier)
        {
            try
            {
                using var db = await _dbFactory.CreateDbContextAsync();
                db.Suppliers.Update(supplier);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"更新供应商 {supplier.Id} 失败");
                return false;
            }
        }

        // 加载供应商当前证书（支持证书记录级别的引用）
        private async Task LoadSupplierCurrentCertificate(ApplicationDbContext db, Supplier supplier, string lampId, CertificateType currentCertType)
        {
            // 获取对应类型的证书记录
            var record = supplier.CertificateRecords?.FirstOrDefault(r => r.CertificateType == currentCertType);
            
            if (record != null)
            {
                // 如果是引用证书记录，优先使用被引用记录的最新证书
                if (record.UsingReference && record.ReferenceCertificateRecordId != null && record.ReferenceCertificateRecord != null)
                {
                    if (record.ReferenceCertificateRecord.LatestCertificateId != null)
                    {
                        supplier.CurrentCertificate = await db.Certificates.FindAsync(record.ReferenceCertificateRecord.LatestCertificateId);
                        supplier.CurrentCertificateId = record.ReferenceCertificateRecord.LatestCertificateId;
                        return;
                    }
                }
                // 使用自身证书记录的最新证书
                else if (record.LatestCertificateId != null)
                {
                    supplier.CurrentCertificate = await db.Certificates.FindAsync(record.LatestCertificateId);
                    supplier.CurrentCertificateId = record.LatestCertificateId;
                    return;
                }
            }
            
            // 若未找到对应类型的证书，清空当前证书
            supplier.CurrentCertificate = null;
            supplier.CurrentCertificateId = null;
        }
        
        // 根据引用关系获取证书记录的所有证书
        private List<Certificate> GetCertificateRecordCertificates(CertificateRecord record)
        {
            // 如果是引用证书记录，返回被引用记录的所有证书
            if (record.UsingReference && record.ReferenceCertificateRecordId != null && record.ReferenceCertificateRecord != null)
            {
                return record.ReferenceCertificateRecord.Certificates.ToList();
            }
            // 非引用类型的证书记录，返回自身的所有证书
            else
            {
                return record.Certificates.ToList();
            }
        }

        public async Task SaveChangesAsync(List<Supplier> suppliers)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            db.Suppliers.UpdateRange(suppliers);
            await db.SaveChangesAsync();
        }
    }
}
