namespace PricingService.Domain.Entities
{
    public class ProductPriceEntity : BaseEntity
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public DateTime SetDate { get; set; } = DateTime.Now;

        public int ProductDetailId { get; set; }
    }
}
