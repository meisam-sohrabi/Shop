using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Domain.Entities;

namespace ProductService.Infrastructure.EntityFrameWorkCore.EntityConfigurations
{
    public class CategoryEntityConfiguration : IEntityTypeConfiguration<CategoryEntity>
    {
        public void Configure(EntityTypeBuilder<CategoryEntity> builder)
        {
            builder.ToTable("Categories","Product");
            builder.HasKey(e => e.Id); // category can be unique 
            builder.Property(e => e.Name).IsRequired().HasMaxLength(50);
        }
    }
}
