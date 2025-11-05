using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AccountService.Domain.Entities;

namespace AccountService.Infrastructure.EntityFrameWorkCore.EntityConfigurations
{
    public class CustomUserEntityConfiguration : IEntityTypeConfiguration<CustomUserEntity>
    {
        public void Configure(EntityTypeBuilder<CustomUserEntity> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(c=>c.FullName).IsRequired().HasMaxLength(50);
        }
    }
}
