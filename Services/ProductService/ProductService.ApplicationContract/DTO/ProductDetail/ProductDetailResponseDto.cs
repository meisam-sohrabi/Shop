namespace ProductService.ApplicationContract.DTO.ProductDetail
{
    public class ProductDetailResponseDto
    {
        public int Id { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; }
    }
}
