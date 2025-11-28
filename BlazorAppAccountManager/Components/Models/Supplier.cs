using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorAppAccountManager.Components.Models
{
    public class Supplier
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string LampId { get; set; } // 关联的灯具ID

        [Required]
        public string Name { get; set; } // 供应商名称

        public Lamp Lamp { get; set; }

        [NotMapped]
        public string? CurrentCertificateId { get; set; }

        public string? ReferenceSupplierId { get; set; } // 参考证书ID（可为空）

        [NotMapped]
        public Certificate? CurrentCertificate { get; set; }

        public bool UsingReference { get; set; } = false;

        // 导航属性：供应商的所有证书记录（按类型区分）
        public ICollection<CertificateRecord> CertificateRecords { get; set; } = new List<CertificateRecord>();

    }
}
