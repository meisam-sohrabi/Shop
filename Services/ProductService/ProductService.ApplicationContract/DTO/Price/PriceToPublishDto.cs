namespace ProductService.ApplicationContract.DTO.Price
{
    public class PriceToPublishDto
    {
        public string UserId { get; set; }
        public decimal Price { get; set; }
        public int ProductDetailId { get; set; }
    }
}
