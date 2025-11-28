using BlazorAppAccountManager.Components.Models;
using Microsoft.AspNetCore.Identity;

namespace BlazorAppAccountManager.Components.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        // 导航属性：用户创建的项目
        public ICollection<Project> CreatedProjects { get; set; } = new List<Project>();
    }

}
