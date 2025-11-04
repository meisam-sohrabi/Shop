using ProductService.ApplicationContract.DTO.ProductDetail;

namespace ProductService.ApplicationContract.DTO.Search
{
    public class SearchResponseDto
    {
        public string categoryName { get; set; }
        public string productBrand { get; set; }
        public string produtName { get; set; }
        public string productColor { get; set; }
        public string productSize { get; set; }
        public decimal Price { get; set; }
    }
}
