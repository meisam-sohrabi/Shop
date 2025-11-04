using GatewayService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using BaseConfig;
namespace GatewayService.Infrastructure.EntityFrameWorkCore.AppDbContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ApplicaitonConfiguration.gateway_accountConnectionSqlString);
            base.OnConfiguring(optionsBuilder);
        }
        public DbSet<RefreshTokenEntity> RefreshToken { get; set; }
        public DbSet<UserSessionEntity> UserSessions { get; set; }
        public DbSet<CustomUserEntity> Users { get; set; }
        public DbSet<CustomRoleEntity> Roles { get; set; }
        public DbSet<CustomUserRoleEntity> UserRoles { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(builder);
        }
    }
}
