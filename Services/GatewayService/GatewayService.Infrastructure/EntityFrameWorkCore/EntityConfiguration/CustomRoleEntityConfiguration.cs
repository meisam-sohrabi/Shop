using GatewayService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GatewayService.Infrastructure.EntityFrameWorkCore.EntityConfiguration
{
    public class CustomRoleEntityConfiguration : IEntityTypeConfiguration<CustomRoleEntity>
    {
        public void Configure(EntityTypeBuilder<CustomRoleEntity> builder)
        {
            builder.HasKey(c => c.Id);
            builder.ToTable("AspNetRoles", t => t.ExcludeFromMigrations());
        }
    }
}
