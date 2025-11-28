using BlazorAppAccountManager.Components.Data;
using System.ComponentModel.DataAnnotations;

namespace BlazorAppAccountManager.Components.Models
{
    public class Project
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Name { get; set; } = "T1E";
        
        public DateTime SopTime { get; set; } // 示例默认值，可根据实际改
        
        public List<Lamp> Lamps { get; set; } = new();

        public DateTime CreateTime { get; set; }

        // 新增：外键 - 创建者ID
        public string CreatorId { get; set; }
        // 导航属性：项目创建者
        public ApplicationUser Creator { get; set; }
    }
}
