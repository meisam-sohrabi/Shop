using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Domain.Entities;

namespace AccountService.Infrastructure.EntityFrameWorkCore.EntityConfigurations
{
    public class PermissionEntityConfiguration : IEntityTypeConfiguration<LocalPermissionEntity>
    {
        public void Configure(EntityTypeBuilder<LocalPermissionEntity> builder)
        {
            builder.ToTable("Permissions","User");
            builder.HasKey(p => p.Id);
            builder.Property(c=> c.Id).ValueGeneratedNever();
            builder.Property(p => p.Resource).IsRequired();
            builder.Property(p => p.Action).IsRequired();
        }
    }
}
