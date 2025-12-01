using AccountService.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BaseConfig;
using MassTransit;
namespace AccountService.Infrastructure.EntityFrameWorkCore.AppDbContext
{
    public class ApplicationDbContext : IdentityDbContext<CustomUserEntity>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ApplicaitonConfiguration.gateway_accountConnectionSqlString);
            base.OnConfiguring(optionsBuilder);
        }
        public DbSet<PermissionEntity> Permissions { get; set; }
        public DbSet<UserPermissoinEntity> UserPermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            builder.AddInboxStateEntity();
            builder.AddOutboxMessageEntity();
            builder.AddOutboxStateEntity();
            base.OnModelCreating(builder);
        }
    }
}
