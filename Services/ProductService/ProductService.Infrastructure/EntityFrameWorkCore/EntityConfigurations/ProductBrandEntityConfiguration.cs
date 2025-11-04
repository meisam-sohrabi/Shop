using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Domain.Entities;

namespace ProductService.Infrastructure.EntityFrameWorkCore.EntityConfigurations
{
    public class ProductBrandEntityConfiguration : IEntityTypeConfiguration<ProductBrandEntity>
    {
        public void Configure(EntityTypeBuilder<ProductBrandEntity> builder)
        {
            builder.ToTable("Brands","Product");
            builder.HasKey(e => e.Id);// ProductBrand can be unique 
            builder.Property(p => p.Name).IsRequired().HasMaxLength(30);
            builder.Property(p => p.Description).HasMaxLength(350);
            builder.HasMany(e => e.Products)
                .WithOne(p => p.ProductBrand)
                .HasForeignKey(p => p.ProductBrandId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
