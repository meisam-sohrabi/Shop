using ProductService.ApplicationContract.DTO.Base;
using ProductService.ApplicationContract.DTO.ProductBrand;

namespace ProductService.ApplicationContract.Interfaces.ProductBrand
{
    public interface IProductBrandAppService
    {
        Task<BaseResponseDto<ProductBrandDto>> CreateProductBrand(ProductBrandDto productBrandDto);
        Task<BaseResponseDto<ProductBrandDto>> EditProductBrand(int id, ProductBrandDto productBrandDto);
        Task<BaseResponseDto<List<ProductBrandDto>>> GetAllProductBrands();
        Task<BaseResponseDto<ProductBrandDto>> GetProductBrand(int id);
        Task<BaseResponseDto<ProductBrandDto>> DeleteProductBrand(int id);
    }
}
