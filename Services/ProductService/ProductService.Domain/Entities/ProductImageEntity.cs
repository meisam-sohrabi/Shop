namespace ProductService.Domain.Entities
{
    // without annotaion just using fluent api 
    public class ProductImageEntity  : BaseEntity
    {
        public int Id { get; set; }
        public string? ImageUrl { get; set; }
        public int ProductDetailId { get; set; }
        public ProductDetailEntity? ProductDetail { get; set; }

    }
}
