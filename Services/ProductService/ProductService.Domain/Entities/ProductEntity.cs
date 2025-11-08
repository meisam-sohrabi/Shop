namespace ProductService.Domain.Entities
{
    public class ProductEntity : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int CategoryId { get; set; }
        public CategoryEntity Category { get; set; }

        public int ProductBrandId { get; set; }
        public ProductBrandEntity ProductBrand { get; set; }

        public ICollection<ProductDetailEntity> ProductDetails { get; set; } = new List<ProductDetailEntity>();
    }
}
