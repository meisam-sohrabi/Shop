using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PricingService.Domain.Entities;

namespace PricingService.Infrastructure.EntityFrameWorkCore.EntityConfigurations
{
    public class ProductPriceEntityConfiguration : IEntityTypeConfiguration<ProductPriceEntity>
    {
        public void Configure(EntityTypeBuilder<ProductPriceEntity> builder)
        {
            builder.ToTable("ProductPrice","Price");
            builder.HasKey(k => k.Id);
            builder.Property(p=> p.Price).HasPrecision(18,2).IsRequired();

        }
    }
}
