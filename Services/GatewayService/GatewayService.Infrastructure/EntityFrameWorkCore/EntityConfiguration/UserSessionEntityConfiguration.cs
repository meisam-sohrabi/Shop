using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GatewayService.Domain.Entities;

namespace GatewayService.Infrastructure.EntityFrameWorkCore.EntityConfiguration
{
    public class UserSessionEntityConfiguration : IEntityTypeConfiguration<UserSessionEntity>
    {
        public void Configure(EntityTypeBuilder<UserSessionEntity> builder)
        {
            builder.ToTable("UserSessions");
            builder.HasKey(e => e.Id);
        }
    }
}
