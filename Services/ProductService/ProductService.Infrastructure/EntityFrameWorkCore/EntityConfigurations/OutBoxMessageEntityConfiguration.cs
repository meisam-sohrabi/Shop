using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Domain.Entities;

namespace AccountService.Infrastructure.EntityFrameWorkCore.EntityConfigurations
{
    internal class OutBoxMessageEntityConfiguration : IEntityTypeConfiguration<OutBoxMessagesEntity>
    {
        public void Configure(EntityTypeBuilder<OutBoxMessagesEntity> builder)
        {
            builder.ToTable("OutBoxMessage", "Product");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Event).IsRequired();
            builder.Property(c => c.Content).IsRequired();
        }
    }
}
