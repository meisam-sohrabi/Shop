using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Domain.Entities;

namespace ProductService.Infrastructure.EntityFrameWorkCore.EntityConfigurations
{
    public class UserPermissionEntityConfiguration : IEntityTypeConfiguration<LocalUserPermissionEntity>
    {
        public void Configure(EntityTypeBuilder<LocalUserPermissionEntity> builder)
        {
            builder.ToTable("UserPermissions","User");
            builder.HasKey(up => new { up.PermissionId, up.UserId });

        }
    }
}
