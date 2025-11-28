using System.ComponentModel.DataAnnotations;
namespace BlazorAppAccountManager.Components.Models
{
    public class LampTemplate
    {
        // 主键（唯一标识每个车灯）
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        // 车灯名称（如“远光”“近光”）
        public string Name { get; set; } = string.Empty;

        // 排序序号（越小越靠前，拖拽后更新此值）
        public int SortOrder { get; set; }
    }
}
