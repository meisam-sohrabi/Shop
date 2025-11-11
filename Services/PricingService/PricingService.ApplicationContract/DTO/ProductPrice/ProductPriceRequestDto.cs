namespace PricingService.ApplicationContract.DTO.ProductPrice
{
    public class ProductPriceRequestDto
    {
        public decimal Price { get; set; }
        public int ProductDetailId { get; set; }
    }
}
