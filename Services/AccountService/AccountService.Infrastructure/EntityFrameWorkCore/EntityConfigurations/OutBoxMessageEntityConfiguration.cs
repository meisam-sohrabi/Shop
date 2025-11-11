using AccountService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountService.Infrastructure.EntityFrameWorkCore.EntityConfigurations
{
    internal class OutBoxMessageEntityConfiguration : IEntityTypeConfiguration<OutBoxMessageEntity>
    {
        public void Configure(EntityTypeBuilder<OutBoxMessageEntity> builder)
        {
            builder.ToTable("OutBoxMessage", "User");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Event).IsRequired();
            builder.Property(c => c.Content).IsRequired();
        }
    }
}
