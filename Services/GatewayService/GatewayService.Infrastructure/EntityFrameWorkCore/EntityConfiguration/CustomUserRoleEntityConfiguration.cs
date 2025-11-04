using GatewayService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GatewayService.Infrastructure.EntityFrameWorkCore.EntityConfiguration
{
    internal class CustomUserRoleEntityConfiguration : IEntityTypeConfiguration<CustomUserRoleEntity>
    {
        public void Configure(EntityTypeBuilder<CustomUserRoleEntity> builder)
        {
            builder.HasKey(c => new { c.RoleId, c.UserId });
            builder.ToTable("AspNetUserRoles", t => t.ExcludeFromMigrations());
        }
    }
}
