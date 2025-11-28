using BlazorAppAccountManager.Components.Data;
using BlazorAppAccountManager.Components.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlazorAppAccountManager.Components.Services
{
    /// <summary>
    /// 项目服务类，负责处理项目相关的数据访问和业务逻辑
    /// </summary>
    public class ProjectService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbFactory">数据库上下文工厂</param>
        /// <param name="userManager">用户管理器</param>
        public ProjectService(IDbContextFactory<ApplicationDbContext> dbFactory, UserManager<ApplicationUser> userManager)
        {
            _dbFactory = dbFactory;
            _userManager = userManager;
        }

        /// <summary>
        /// 加载项目列表
        /// </summary>
        /// <param name="currentUserId">当前用户ID</param>
        /// <param name="isAdmin">当前用户是否为管理员</param>
        /// <returns>项目列表</returns>
        public async Task<List<Project>> LoadProjectsAsync(string currentUserId, bool isAdmin)
        {
            // 参数验证
            if (string.IsNullOrEmpty(currentUserId))
            {
                return new List<Project>();
            }

            using var db = await _dbFactory.CreateDbContextAsync();

            // 基础查询，包含必要的导航属性
            // 注意：不直接在查询中加载CertificateRecords，而是在后续处理时根据需要加载
            IQueryable<Project> baseQuery = db.Projects
                .Include(p => p.Lamps)
                    .ThenInclude(l => l.Suppliers)
                .OrderBy(p => p.CreateTime);

            // 根据角色应用过滤条件
            if (!isAdmin)
            {
                // 普通用户只加载自己的项目
                baseQuery = baseQuery.Where(p => p.CreatorId == currentUserId);
            }

            // 执行查询并获取结果
            var results = await baseQuery
                .Select(p => new 
                {
                    Project = p,
                    CreatorName = db.Users
                        .Where(u => u.Id == p.CreatorId)
                        .Select(u => u.UserName)
                        .FirstOrDefault(),
                    IsCurrentUser = p.CreatorId == currentUserId
                })
                .ToListAsync();

            // 处理结果数据
            var projects = new List<Project>();
            foreach (var result in results)
            {
                var project = result.Project;

                // 排序灯具子集合
                project.Lamps = project.Lamps
                    .OrderBy(lamp => lamp.SortOrder)
                    .ThenBy(lamp => lamp.Name)
                    .ToList();

                // 处理每个灯具的供应商，支持证书记录级别的引用
                foreach (var lamp in project.Lamps)
                {
                    // 处理每个供应商
                    foreach (var supplier in lamp.Suppliers)
                    {
                        // 加载供应商的证书记录，包括引用关系
                        supplier.CertificateRecords = await db.CertificateRecords
                            .Where(cr => cr.SupplierId == supplier.Id)
                            .Include(cr => cr.Certificates)
                            .Include(cr => cr.ReferenceCertificateRecord)
                                .ThenInclude(rcr => rcr.Certificates)
                            .ToListAsync();
                    }
                }

                // 管理员逻辑：为其他用户的项目添加前缀
                if (isAdmin && !result.IsCurrentUser)
                {
                    project.Name = $"{project.Name}-{result.CreatorName ?? "未知用户"}";
                }

                projects.Add(project);
            }

            return projects;
        }

        /// <summary>
        /// 创建新项目
        /// </summary>
        /// <param name="project">项目对象</param>
        /// <param name="creatorId">创建者ID</param>
        /// <returns>创建的项目</returns>
        public async Task<Project> CreateProjectAsync(Project project, string creatorId)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            
            project.CreatorId = creatorId;
            project.CreateTime = DateTime.Now;
            
            await db.Projects.AddAsync(project);
            await db.SaveChangesAsync();
            
            return project;
        }

        /// <summary>
        /// 删除项目
        /// </summary>
        /// <param name="projectId">项目ID</param>
        /// <returns>是否删除成功</returns>
        public async Task<bool> DeleteProjectAsync(string projectId)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            
            var project = await db.Projects.FindAsync(projectId);
            if (project == null)
            {
                return false;
            }
            
            db.Projects.Remove(project);
            await db.SaveChangesAsync();
            
            return true;
        }
        
        /// <summary>
        /// 获取当前用户的认证状态信息
        /// </summary>
        /// <param name="user">当前用户</param>
        /// <returns>包含用户ID和是否为管理员的元组</returns>
        public async Task<(string UserId, bool IsAdmin)> GetUserAuthInfoAsync(ClaimsPrincipal user)
        {
            var currentUserId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            const string AdminRoleName = "Admin";
            bool isAdmin = user.IsInRole(AdminRoleName);
            
            return (currentUserId, isAdmin);
        }
    }
}