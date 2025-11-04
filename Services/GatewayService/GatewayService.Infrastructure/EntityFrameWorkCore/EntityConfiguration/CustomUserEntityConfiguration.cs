using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GatewayService.Domain.Entities;

namespace GatewayService.Infrastructure.EntityFrameWorkCore.EntityConfiguration
{
    public class CustomUserEntityConfiguration : IEntityTypeConfiguration<CustomUserEntity>
    {
        public void Configure(EntityTypeBuilder<CustomUserEntity> builder)
        {
            builder.HasKey(c => c.Id);
            builder.ToTable("AspNetUsers", c => c.ExcludeFromMigrations());
        }
    }
}
