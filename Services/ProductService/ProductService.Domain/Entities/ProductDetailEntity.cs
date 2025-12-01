namespace ProductService.Domain.Entities
{
    public class ProductDetailEntity : BaseEntity
    {
        public int Id { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public byte[] RowVersion { get; set; }

        public int ProductId { get; set; }
        public ProductEntity Product { get; set; }

        public ICollection<ProductImageEntity> ProductImages { get; set; } = new List<ProductImageEntity>();

    }
}
