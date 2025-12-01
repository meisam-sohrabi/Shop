using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Domain.Entities;

namespace ProductService.Infrastructure.EntityFrameWorkCore.EntityConfigurations
{
    public class ProductDetailEntityConfiguration : IEntityTypeConfiguration<ProductDetailEntity>
    {
        public void Configure(EntityTypeBuilder<ProductDetailEntity> builder)
        {
            builder.ToTable("Details","Product");
            builder.HasKey(e => e.Id);
            builder.Property(p => p.Size).IsRequired().HasMaxLength(30);
            builder.Property(c => c.Quantity).IsRequired();
            builder.Property(p => p.Description).HasMaxLength(350);
            builder.Property(r => r.RowVersion).IsRowVersion();
            builder.HasOne(p => p.Product)
                .WithMany(p => p.ProductDetails)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
