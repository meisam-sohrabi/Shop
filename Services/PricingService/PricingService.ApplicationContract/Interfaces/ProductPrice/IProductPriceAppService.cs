using PricingService.ApplicationContract.DTO.Base;
using PricingService.ApplicationContract.DTO.ProductPrice;

namespace PricingService.ApplicationContract.Interfaces.ProductPrice
{
    public interface IProductPriceAppService
    {
        Task<BaseResponseDto<ProductPriceResponseDto>> CreateProductPrice(ProductPriceRequestDto productPriceRequestDto);
        Task<BaseResponseDto<ProductPriceResponseDto>> EditProductPrice(int id, ProductPriceRequestDto productPriceRequestDto);
        Task<BaseResponseDto<ProductPriceResponseDto>> DeleteProductPrice(int id);
        Task<BaseResponseDto<List<ProductPriceResponseDto>>> GetProductPrice(int id);
        //Task<BaseResponseDto<List<ProductPriceResponseDto>>> GetAllProductPrice();
    }
}
