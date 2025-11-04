using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InventoryService.Domain.Entities;

namespace InventoryService.Infrastructure.EntityFrameWorkCore.EntityConfigurations
{
    public class ProductInventoryEntityConfiguration : IEntityTypeConfiguration<ProductInventoryEntity>
    {
        public void Configure(EntityTypeBuilder<ProductInventoryEntity> builder)
        {
            builder.ToTable("Inventory","Inventory");
            builder.HasKey(k => k.Id);
            builder.Property(p => p.QuantityChange).IsRequired();
        }
    }
}
