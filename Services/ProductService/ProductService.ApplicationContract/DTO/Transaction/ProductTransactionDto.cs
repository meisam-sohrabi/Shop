using ProductService.ApplicationContract.DTO.Product;
using ProductService.ApplicationContract.DTO.ProductDetail;
using ProductService.ApplicationContract.DTO.ProductImage;

namespace ProductService.ApplicationContract.DTO.Transaction
{
    public class ProductTransactionDto
    {
        public ProductRequestDto Product { get; set; }
        public ProductDetailRequestDto ProductDetail { get; set; }
        public ProdcutImageRequestDto? ProductImage { get; set; }
        public decimal Price { get; set; }
    }
}