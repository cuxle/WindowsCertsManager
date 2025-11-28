using BlazorAppAccountManager.Components.Data;
using BlazorAppAccountManager.Components.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace BlazorAppAccountManager.Components.Services
{
    public class CertificateService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;
        private readonly ILogger<CertificateService> _logger;

        // 通过依赖注入获取数据库工厂和日志
        public CertificateService(IDbContextFactory<ApplicationDbContext> dbFactory, ILogger<CertificateService> logger)
        {
            _dbFactory = dbFactory;
            _logger = logger;
        }

        public async Task<(bool IsSuccess, string ErrorMessage)>
        AddCertificateToSupplierAsync(Certificate cert, string supplierId, PdfDocument currentPdfDocument, CertificateType currentCertificateType)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            try
            {
                // 1. 校验供应商是否存在（并加载已有证书记录）
                var trackedSupplier = await db.Suppliers
                    .Include(s => s.CertificateRecords)
                    .FirstOrDefaultAsync(s => s.Id == supplierId);

                if (trackedSupplier == null)
                {
                    return (false, $"数据库中未找到ID为 {supplierId} 的供应商");
                }

                // 2. 校验1：同供应商下不能有相同证书编号和版本的证书
                var existingCertInSameSupplier = await db.Certificates
                    .FirstOrDefaultAsync(c => c.SupplierId == supplierId && c.Number == cert.Number && c.Version == cert.Version);

                if (existingCertInSameSupplier != null)
                {
                    var errorMsg = $"同一供应商下证书编号 '<span style=\"color:red;\">{cert.Number}</span>' 版本 '<span style=\"color:red;\">{cert.Version}</span>' 已存在，无需重复添加";
                    return (false, errorMsg);
                }

                // 3. 校验2：不同供应商下不能加载相同编号的证书
                var existingCertInDifferentSupplier = await db.Certificates
                    .Include(c => c.Supplier)
                    .FirstOrDefaultAsync(c => c.SupplierId != supplierId && c.Number == cert.Number);

                if (existingCertInDifferentSupplier != null)
                {
                    var supplierName = existingCertInDifferentSupplier.Supplier?.Name ?? "未知供应商";
                    var errorMsg = $"证书编号 '<span style=\"color:red;\">{cert.Number}</span>' 已存在于其他供应商 '<span style=\"color:red;\">{supplierName}</span>' 下，不允许跨供应商使用相同证书编号";
                    return (false, errorMsg);
                }

                // 3. 处理证书：新建或复用已有证书（按ID判断）
                var certificate = await db.Certificates.FindAsync(cert.Id) ?? cert;

                // 4. 若为新证书：设置关联关系（供应商、PDF文档）
                if (certificate.Id == cert.Id && !db.Certificates.Local.Contains(certificate))
                {
                    certificate.SupplierId = trackedSupplier.Id;
                    certificate.Supplier = trackedSupplier;

                    // 关联PDF文档（如有）
                    if (currentPdfDocument != null)
                    {
                        db.PdfDocuments.Add(currentPdfDocument);
                        certificate.PdfDocumentId = currentPdfDocument.Id;
                    }

                    // 更新供应商当前证书信息
                    trackedSupplier.CurrentCertificateId = certificate.Id;
                    trackedSupplier.UsingReference = false;
                    db.Suppliers.Update(trackedSupplier);

                    // 5. 处理证书记录（CertificateRecord）：新建或更新
                    var certRecord = trackedSupplier.CertificateRecords
                        ?.FirstOrDefault(r => r.CertificateType == currentCertificateType)
                        ?? new CertificateRecord
                        {
                            CertificateType = currentCertificateType,
                            SupplierId = trackedSupplier.Id,
                            Supplier = trackedSupplier,
                            Certificates = new List<Certificate>()
                        };

                    // 添加证书到记录，并更新最新证书
                    certRecord.Certificates.Add(certificate);
                    certRecord.RecalculateLatestCertificate();

                    // 保存证书记录（更新或新增）
                    if (db.CertificateRecords.Local.Any(r => r.Id == certRecord.Id))
                    {
                        db.CertificateRecords.Update(certRecord);
                    }
                    else
                    {
                        db.CertificateRecords.Add(certRecord);
                    }

                    // 6. 保存新证书到数据库
                    db.Certificates.Add(certificate);
                    await db.SaveChangesAsync();
                }

                return (true, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "添加证书到供应商时发生异常");
                return (false, $"数据处理失败：{ex.Message}");
            }
        }


        // 检查证书是否被其他项目引用或是否是记录的最后一个且被引用
    public async Task<(bool HasReference, List<string> References, bool IsLastCertificateReferenced)> CheckCertificateReferences(string certificateId)
    {
        try
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            
            var references = new List<string>();
            bool isLastCertificateReferenced = false;
            
            // 获取证书对应的证书记录
            var certificate = await db.Certificates           
                .FirstOrDefaultAsync(c => c.Id == certificateId);
            
            if (certificate == null)
            {
                return (false, references, false);
            }
            
            // 查找引用了该证书记录的其他证书记录
            var referencedByRecords = await db.CertificateRecords
                .Where(cr => cr.ReferenceCertificateRecordId == certificate.CertificateRecordId)
                .Include(cr => cr.Supplier)
                .ToListAsync();
            
            // 获取证书记录及其关联的所有证书
            var certRecord = await db.CertificateRecords
                .Include(r => r.Certificates)
                .FirstOrDefaultAsync(r => r.Id == certificate.CertificateRecordId);
            
            // 检查是否是CertificateRecord中的最后一个证书
            bool isLastCertificateInRecord = certRecord != null && certRecord.Certificates.Count <= 1;
            
            // 如果是最后一个证书且被其他记录引用
            if (isLastCertificateInRecord && referencedByRecords.Any())
            {
                isLastCertificateReferenced = true;
                references.Add("此证书是其证书记录的最后一个且被其他证书记录引用");
            }

            return (references.Any(), references, isLastCertificateReferenced);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"检查证书引用时发生错误：{certificateId}");
            return (false, new List<string>(), false);
        }
    }
    
    // 检查供应商的证书是否被其他项目引用 - 简化版
    public async Task<(bool HasReference, List<string> References)> CheckSupplierCertificateReferences(string supplierId)
    {
        try
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            
            // 基于实际模型关系查询：通过Supplier关联到Lamp和Certificate/CertificateRecord
            var references = new List<string>();
            
            // 获取该供应商的所有证书
            var certificates = await db.Certificates
                .Where(c => c.SupplierId == supplierId)
                .ToListAsync();            

            
            // 检查其他证书记录是否引用了该供应商的证书记录
            var referencedByOtherRecords = await db.CertificateRecords
                .Where(cr => cr.ReferenceCertificateRecordId != null && 
                            cr.ReferenceCertificateRecord.SupplierId == supplierId)
                .Include(cr => cr.ReferenceCertificateRecord)
                .Include(cr => cr.Supplier)
                .ToListAsync();
            
            foreach (var record in referencedByOtherRecords)
            {
                references.Add($"证书记录 {record.ReferenceCertificateRecord.CertificateType} - 被供应商 {record.Supplier.Name} 的证书记录引用");
            }
            
            return (references.Any(), references);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"检查供应商证书引用时发生错误：{supplierId}");
            return (false, new List<string>());
        }
    }
    
    public async Task<(bool isSuccess, string errorMsg)> RemoveCertificate(Supplier supplier, string certId)
    {
        // 1. 参数验证（提前拦截无效输入）
        if (supplier == null)
            return (false, "供应商对象不能为null");

        if (string.IsNullOrWhiteSpace(certId))
            return (false, "证书ID不能为空或空白");

        using var db = await _dbFactory.CreateDbContextAsync();
        try
        {
            var certToDelete = await db.Certificates
                .FirstOrDefaultAsync(s => s.Id == certId);
            if (certToDelete == null)
                return (false, $"{certId}不在数据库中");

            var certRecord = await db.CertificateRecords
                .Include(r => r.Certificates)
                .FirstOrDefaultAsync(s => s.Id == certToDelete.CertificateRecordId);

            if (certRecord == null)
                return (false, "未找到所在CertRecord容器");

            // 4. 验证证书记录是否属于当前供应商（防止跨供应商删除）
            if (certRecord.SupplierId != supplier.Id)
                return (false, $"证书[{certId}]不属于当前供应商，无法删除");

            if (certRecord.Certificates.Any())
            {
                var certInRecord = certRecord.Certificates.FirstOrDefault(s => s.Id == certId);
                if (certInRecord != null)
                {
                    certRecord.Certificates.Remove(certInRecord);
                    certRecord.RecalculateLatestCertificate();
                } else
                {
                    // 异常情况：数据库中存在证书，但未在记录的Certificates集合中（可能是数据不一致）
                    _logger?.LogWarning($"证书[{certId}]存在于数据库，但未在其所属记录[{certRecord.Id}]的Certificates集合中");
                }      
            }

            // 6. 执行数据库删除和更新操作
            db.Certificates.Remove(certToDelete);
            db.CertificateRecords.Update(certRecord);

            await db.SaveChangesAsync();

            // 7. 操作成功
            _logger?.LogInformation($"证书[{certId}]已成功删除，所属记录[{certRecord.Id}]已更新");

            return (true, null);
        }
        catch (DbUpdateException ex)
        {
            // 数据库更新异常（如外键约束、并发冲突等）
            var innerMsg = ex.InnerException?.Message ?? ex.Message;
            _logger?.LogError(ex, $"删除证书[{certId}]时数据库操作失败：{innerMsg}");
            return (false, $"数据库操作失败：{innerMsg}");
        }
        catch (Exception ex)
        {
            // 其他未预料的异常
            _logger?.LogError(ex, $"删除证书[{certId}]时发生错误");
            return (false, $"操作失败：{ex.Message}");
        }
    }
    }
}
