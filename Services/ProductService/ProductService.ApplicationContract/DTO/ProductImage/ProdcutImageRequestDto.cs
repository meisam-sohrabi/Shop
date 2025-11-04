using Microsoft.AspNetCore.Http;

namespace ProductService.ApplicationContract.DTO.ProductImage
{
    public class ProdcutImageRequestDto
    {
        public IFormFile? ImageUrl { get; set; }
        public int ProductDetailId { get; set; }
    }
}
