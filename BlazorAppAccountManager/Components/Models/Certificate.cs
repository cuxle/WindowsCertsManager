using System.ComponentModel.DataAnnotations;

namespace BlazorAppAccountManager.Components.Models
{
    public class Certificate : ICloneable
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string SupplierId { get; set; } // 外键

        [Required]
        public string CertificateRecordId { get; set; }

        [Required]
        public string Number { get; set; } // 证书编号

        public string? ReferenceCertifateId { get; set; } // 参考证书ID（可为空）

        public string Version { get; set; } //版本号

        public string PdfDocumentId { get; set; }

        public DateTime CertificationTime { get; set; } // 认证时间

        public DateTime ExpirationTime { get; set; } // 过期时间

        public DateTime CreateTime { get; set; } = DateTime.Now;

        public Supplier Supplier { get; set; }

        // 替换原来的IsEMark布尔值
        [Required] // 建议添加必填属性
        public CertificateType Type { get; set; } = CertificateType.Other; // 设置默认值

        // 实现ICloneable接口的Clone方法（深拷贝）
        public object Clone()
        {
            return new Certificate
            {
                // 基础字段直接复制值
                Id = Guid.NewGuid().ToString(), // 注意：如需新ID可改为 Guid.NewGuid().ToString()
                SupplierId = null,
                CertificateRecordId = null,
                Number = this.Number,
                Version = this.Version,
                PdfDocumentId = this.PdfDocumentId,
                CertificationTime = this.CertificationTime,
                ExpirationTime = this.ExpirationTime,
                CreateTime = this.CreateTime, // 或根据需求重置为当前时间
                Type = this.Type,

                // 导航属性处理：
                // 1. 若不需要复制导航对象，设为null（避免引用原Supplier）
                Supplier = null
                // 2. 若需要复制导航对象，需确保Supplier也实现了ICloneable
                // Supplier = this.Supplier?.Clone() as Supplier
            };
        }
    }

    public enum CertificateType
    {
        EMark,
        CCC,
        强检,
        自我声明,
        自愿性认证,
        Other
    }
}
