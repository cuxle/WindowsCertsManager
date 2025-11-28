using BlazorAppAccountManager.Components.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlazorAppAccountManager.Components.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {

        public DbSet<Project> Projects { get; set; }

        // 映射到数据库的Lamps表
        public DbSet<Lamp> Lamps { get; set; }

        // 将Supplier实体映射到数据库中的Suppliers表
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<PdfDocument> PdfDocuments { get; set; }

        // 将CertificateRecord实体映射到数据库表
        public DbSet<CertificateRecord> CertificateRecords { get; set; }

        public DbSet<LampTemplate> LampTemplates { get; set; }

        // ��ѡ������ʵ���ϵ�������Լ���������Զ���ȣ�
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. 配置 Project 与 Lamp 的一对多关系，默认级联删除
            modelBuilder.Entity<Project>()
                .HasMany(p => p.Lamps)
                .WithOne(l => l.Project)
                .HasForeignKey(l => l.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // 2. 配置 Lamp 与 Supplier 的一对多关系
            modelBuilder.Entity<Lamp>()
                .HasMany(l => l.Suppliers)
                .WithOne(l => l.Lamp)
                .HasForeignKey(c => c.LampId).
                OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Supplier>()
                .HasMany(s => s.CertificateRecords)       // 一个供应商可以有多个证书记录
                .WithOne(r => r.Supplier)                 // 一个证书记录必须属于一个供应商
                .HasForeignKey(r => r.SupplierId)         // 指定外键
                .OnDelete(DeleteBehavior.Cascade);        // 删除供应商时级联删除证书记录
                
            // 4. 配置CertificateRecord与Certificate的一对多关系
            // 一个证书记录可以有多个证书（当前+历史）
            modelBuilder.Entity<CertificateRecord>()
                .HasMany(r => r.Certificates)
                .WithOne()
                .HasForeignKey(c => c.CertificateRecordId)
                .OnDelete(DeleteBehavior.Cascade); // 删除证书记录时级联删除所有相关证书
                
            // 5. 配置CertificateRecord的自引用关系
            modelBuilder.Entity<CertificateRecord>()
                .HasOne(r => r.ReferenceCertificateRecord) // 一个证书记录可以引用另一个证书记录
                .WithMany()
                .HasForeignKey(r => r.ReferenceCertificateRecordId)
                .OnDelete(DeleteBehavior.NoAction); // 删除被引用记录时不级联删除
                                                   // 6. 配置CertificateRecord的SupplierId+CertificateType为唯一索引
                                                   // 确保一个供应商对于一种证书类型只有一个记录作为当前有效的证书记录
            modelBuilder.Entity<CertificateRecord>()
                .HasIndex(r => new { r.SupplierId, r.CertificateType })
                .IsUnique();

        // 初始化模板数据，对应原来的默认列表
        // 为每个LampTemplate提供静态Id值，避免EF Core动态生成导致模型每次变化
        var defaultLamps = new List<LampTemplate>
        {
            new() { Id = "1", Name = "远光", SortOrder = 1 },
            new() { Id = "2", Name = "近光", SortOrder = 2 },
            new() { Id = "3", Name = "前雾灯", SortOrder = 3 },
            new() { Id = "4", Name = "前转向信号灯", SortOrder = 4 },
            new() { Id = "5", Name = "前位置灯", SortOrder = 5 },
            new() { Id = "6", Name = "前转向灯", SortOrder = 6 },
            new() { Id = "7", Name = "侧灯", SortOrder = 7 },
            new() { Id = "8", Name = "制动灯", SortOrder = 8 },
            new() { Id = "9", Name = "后转向灯", SortOrder = 9 },
            new() { Id = "10", Name = "后位置灯", SortOrder = 10 },
            new() { Id = "11", Name = "后雾灯", SortOrder = 11 },
            new() { Id = "12", Name = "倒车灯", SortOrder = 12 },
            new() { Id = "13", Name = "后位置制动灯", SortOrder = 13 },
            new() { Id = "14", Name = "牌照灯", SortOrder = 14 }
        };

        modelBuilder.Entity<LampTemplate>().HasData(defaultLamps);
        }
    }

}
