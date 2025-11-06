using ProductService.ApplicationContract.DTO.Base;
using ProductService.ApplicationContract.DTO.Product;
using ProductService.ApplicationContract.DTO.Search;
using ProductService.ApplicationContract.DTO.Transaction;

namespace ProductService.ApplicationContract.Interfaces.Product
{
    public interface IProductAppService
    {
        Task<BaseResponseDto<ProductTransactionServiceDto>> ProductTransaction(ProductTransactionServiceDto productTransactionDto);
        Task<BaseResponseDto<ProductResponseDto>> EditProduct(int id, ProductRequestDto productDto);
        Task<BaseResponseDto<ProductResponseDto>> EditArabicToPersianSP(ProductArabicToPersianDto productArabicToPersianDto);
        Task<BaseResponseDto<ProductResponseDto>> DeleteProduct(int id);
        Task<BaseResponseDto<List<ProductResponseDto>>> GetAllProduct();
        Task<BaseResponseDto<ProductResponseDto>> GetProduct(int id);
        Task<BaseResponseDto<List<SearchResponseDto>>> AdvanceSearchProduct(SearchRequestDto searchRequstDto);
        Task<BaseResponseDto<List<ProductWithInventoryDto>>> GetProductWithInventory(string? search, DateTime? start, DateTime? end);
        Task<List<ProductResponseDto>> GetProductsReport();

    }
}
