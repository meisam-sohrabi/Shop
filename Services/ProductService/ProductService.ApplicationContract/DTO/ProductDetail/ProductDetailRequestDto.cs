namespace ProductService.ApplicationContract.DTO.ProductDetail
{
    public class ProductDetailRequestDto
    {
        public string Size { get; set; }
        public string Color { get; set; }
        public string? Description { get; set; }
        public int ProductId { get; set; } 
    }
}
