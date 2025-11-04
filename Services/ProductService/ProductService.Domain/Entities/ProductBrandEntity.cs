namespace ProductService.Domain.Entities
{
    public class ProductBrandEntity : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        public ICollection<ProductEntity> Products { get; set; }
    }
}
