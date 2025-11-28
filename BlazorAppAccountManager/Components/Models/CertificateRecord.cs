using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace BlazorAppAccountManager.Components.Models
{
    public class CertificateRecord
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string SupplierId { get; set; } // 外键：关联供应商（确保一个供应商对一种类型只有一条记录）

        [Required]
        public CertificateType CertificateType { get; set; } // 证书类型（与需求中的“证书类型”对应）

        // 新增：当前最新证书的ID（冗余字段，方便快速查询最新证书，避免频繁关联查询）
        public string LatestCertificateId { get; set; }
        
        // 新增：引用其他证书记录的ID
        public string? ReferenceCertificateRecordId { get; set; }
        
        // 导航属性：被引用的证书记录
        [ForeignKey(nameof(ReferenceCertificateRecordId))]
        public CertificateRecord? ReferenceCertificateRecord { get; set; }
        
        // 是否使用引用的证书记录
        public bool UsingReference { get; set; } = false;

        // 导航属性：关联供应商（一个供应商可有多条不同类型的证书记录）
        public Supplier Supplier { get; set; }

        // 导航属性：该记录下的所有证书（包括最新和历史版本，通过 IsLatest 区分）
        public ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();

        // 导航属性：当前最新证书（通过 LatestCertificateId 关联，简化查询）
        [NotMapped]
        public Certificate LatestCertificate { get; set; }

        public void RecalculateLatestCertificate()
        {
            // 逻辑：找到颁发日期最新的证书
            var latest = Certificates
                .OrderByDescending(c => c.CreateTime) // 假设 IssueDate 决定了哪个是最新
                .FirstOrDefault();

            var sortedCertificates = Certificates.OrderByDescending(c => c.CreateTime).ToList();

            // 打印所有排序后的内容（调试用）
            foreach (var cert in sortedCertificates)
            {
                Console.WriteLine($"证书ID: {cert.Id}, 创建时间: {cert.CreateTime:yyyy-MM-dd HH:mm:ss}");
                // 可根据需要添加其他字段，如证书编号、颁发日期等
            }

            // 更新冗余字段
            LatestCertificateId = latest?.Id ?? string.Empty;
            // ⚠️ 注意：如果你的 Certificates 集合可能为空，LatestCertificateId 应该设为 null 或 string.Empty
        }
    }
}
