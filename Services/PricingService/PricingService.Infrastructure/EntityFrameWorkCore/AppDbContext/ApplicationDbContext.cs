using Microsoft.EntityFrameworkCore;
using PricingService.Domain.Entities;
using BaseConfig;
namespace PricingService.Infrastructure.EntityFrameWorkCore.AppDbContext
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ApplicaitonConfiguration.PriceConnectionSqlString);
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<ProductPriceEntity> ProductPrices { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            #region Commented Old Config
            //modelBuilder.Entity<ProductEntity>().ToTable("Products");
            //modelBuilder.Entity<CategoryEntity>().ToTable("Categories");
            //modelBuilder.Entity<RefreshTokenEntity>().ToTable("RefreshToken");
            //modelBuilder.Entity<UserSessionEntity>().ToTable("UserSessions");
            //modelBuilder.Entity<ProductBrandEntity>().ToTable("ProductBrands");
            //modelBuilder.Entity<ProductDetailEntity>().ToTable("ProductDetails");
            //modelBuilder.Entity<PermissionEntity>().ToTable("Permissions");
            //modelBuilder.Entity<OrderEntity>().ToTable("Orders");
            //modelBuilder.Entity<ProductEntity>(entity =>
            //{
            //    entity.HasKey(e => e.Id);
            //    entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            //    entity.Property(e => e.Description).IsRequired().HasMaxLength(300);
            //    entity.HasOne(e => e.Category)
            //          .WithMany(c => c.Products)
            //          .HasForeignKey(e => e.CategoryId)
            //          .OnDelete(DeleteBehavior.Restrict);
            //});
            //modelBuilder.Entity<CategoryEntity>(entity =>
            //{
            //    entity.HasKey(e => e.Id);
            //    entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            //}); // category can be unique 
            //modelBuilder.Entity<CustomUserEntity>(entity =>
            //{
            //    entity.HasKey(e => e.Id);
            //    entity.HasOne(e => e.RefreshToken).WithOne(e => e.User).OnDelete(DeleteBehavior.Cascade);
            //});
            //modelBuilder.Entity<RefreshTokenEntity>(entity =>
            //{
            //    entity.HasKey(e => e.Id);
            //    entity.Property(e => e.Token).IsRequired();
            //    entity.Property(e => e.UserId).IsRequired();
            //});
            //modelBuilder.Entity<UserSessionEntity>(entity =>
            //{
            //    entity.HasKey(e => e.Id);
            //    entity.HasOne(e => e.User)
            //    .WithMany(e => e.Sessions)
            //    .HasForeignKey(e => e.UserId)
            //    .OnDelete(DeleteBehavior.Restrict);
            //});
            //modelBuilder.Entity<ProductBrandEntity>(entity =>
            //{
            //    entity.HasKey(e => e.Id);
            //    entity.Property(p => p.Name).IsRequired().HasMaxLength(30);
            //    entity.Property(p => p.Description).HasMaxLength(350);
            //    entity.HasMany(e => e.Products).WithOne(p => p.ProductBrand).HasForeignKey(p => p.ProductBrandId).OnDelete(DeleteBehavior.Restrict);
            //});// ProductBrand can be unique 
            //modelBuilder.Entity<ProductDetailEntity>(entity =>
            //{
            //    entity.HasKey(e => e.Id);
            //    entity.Property(p => p.Size).IsRequired().HasMaxLength(30);
            //    entity.Property(p => p.Description).HasMaxLength(350);
            //    entity.Property(p => p.Price).HasPrecision(18, 2);
            //    entity.HasOne(p => p.Product).WithMany(p => p.ProductDetails).HasForeignKey(e => e.ProductId).OnDelete(DeleteBehavior.Cascade);
            //});
            //modelBuilder.Entity<PermissionEntity>(entity =>
            //{
            //    entity.HasKey(p => p.Id);
            //    entity.Property(p => p.Resource).IsRequired();
            //    entity.Property(p => p.Action).IsRequired();
            //});// Permission can be unique 
            //modelBuilder.Entity<UserPermissoinEntity>()
            //    .HasKey(up => new { up.PermissionId, up.UserId });

            //modelBuilder.Entity<UserPermissoinEntity>()
            //    .HasOne(up => up.User)
            //    .WithMany(u => u.UserPermissions)
            //    .HasForeignKey(up => up.UserId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<UserPermissoinEntity>()
            //    .HasOne(up => up.Permission)
            //    .WithMany(p => p.UserPermissions)
            //    .HasForeignKey(up => up.PermissionId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<OrderEntity>(entity =>
            //{
            //    entity.HasKey(o => o.Id);
            //    entity.Property(o=> o.UserId).IsRequired();
            //    entity.Property(o => o.TotalPrice).HasPrecision(18, 2);
            //    entity.HasOne(o => o.User)
            //    .WithMany(o => o.Orders)
            //    .OnDelete(DeleteBehavior.Restrict);
            //});
            #endregion


            base.OnModelCreating(modelBuilder);
        }
    }
}
