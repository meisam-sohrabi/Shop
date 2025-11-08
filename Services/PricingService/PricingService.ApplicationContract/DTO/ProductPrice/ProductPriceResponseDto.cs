namespace PricingService.ApplicationContract.DTO.ProductPrice
{
    public class ProductPriceResponseDto
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public DateTime SetDate { get; set; }
    }
}
