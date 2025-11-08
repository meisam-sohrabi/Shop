using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.EntityFrameWorkCore.EntityConfigurations
{
    internal class OutBoxMessageEntityConfiguration : IEntityTypeConfiguration<OutBoxMessagesEntity>
    {
        public void Configure(EntityTypeBuilder<OutBoxMessagesEntity> builder)
        {
            builder.ToTable("OutBoxMessage", "Order");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Event).IsRequired();
            builder.Property(c => c.Content).IsRequired();
        }
    }
}
