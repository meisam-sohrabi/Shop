using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GatewayService.Domain.Entities;

namespace GatewayService.Infrastructure.EntityFrameWorkCore.EntityConfiguration
{
    public class RefreshTokenEntityConfiguration : IEntityTypeConfiguration<RefreshTokenEntity>
    {
        public void Configure(EntityTypeBuilder<RefreshTokenEntity> builder)
        {
            builder.ToTable("RefreshToken","Auth");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Token).IsRequired();
            builder.Property(e => e.UserId).IsRequired();
            builder.HasOne(r=> r.User).WithOne().HasForeignKey<RefreshTokenEntity>(c=> c.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
