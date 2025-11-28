using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorAppAccountManager.Components.Models
{
    public class Lamp
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string ProjectId { get; set; }

        [Required]
        public int SortOrder { get; set; }

        [Required]
        public string Name { get; set; }

        //public string? CurrentCertificateId { get; set; }

        //public Certificate? CurrentCertificate { get; set; }

        public Project Project { get; set; }

        public List<Supplier>? Suppliers { get; set; } // 导航属性：所属供应商

        [NotMapped]
        public List<Certificate>? CurrentTypeCerts { get; set; }

        [NotMapped]
        public string CurrentContent { get; set; }

        public bool IsDeleted { get; set; } = false; // 默认值为 false
    }
}
