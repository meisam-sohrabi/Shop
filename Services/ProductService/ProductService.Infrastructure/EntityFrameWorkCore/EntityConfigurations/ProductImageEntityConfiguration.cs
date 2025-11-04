using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Domain.Entities;

namespace ProductService.Infrastructure.EntityFrameWorkCore.EntityConfigurations
{
    public class ProductImageEntityConfiguration : IEntityTypeConfiguration<ProductImageEntity>
    {
        public void Configure(EntityTypeBuilder<ProductImageEntity> builder)
        {
            builder.ToTable("Images", "Product");
            builder.HasKey(c=> c.Id);
            builder.Property(c=> c.ImageUrl)
                   .HasMaxLength(400);
            builder.HasOne(c=> c.ProductDetail)
                .WithMany(c=> c.ProductImages)
                .HasForeignKey(c=> c.ProductDetailId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
