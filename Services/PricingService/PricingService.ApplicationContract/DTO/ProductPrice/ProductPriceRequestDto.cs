namespace PricingService.ApplicationContract.DTO.ProductPrice
{
    public class ProductPriceRequestDto
    {
        public string UserId { get; set; }
        public decimal Price { get; set; }
        public int ProductDetailId { get; set; }
    }
}
