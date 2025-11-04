using ProductService.ApplicationContract.DTO.Base;
using ProductService.ApplicationContract.DTO.Category;

namespace ProductService.ApplicationContract.Interfaces.Category
{
    public interface ICategoryAppService
    {
        Task<BaseResponseDto<CategoryDto>> CreateCategory(CategoryDto categoryDto);
        Task<BaseResponseDto<CategoryDto>> EditCategory(int id, CategoryDto categoryDto);
        Task<BaseResponseDto<CategoryDto>> DeleteCategory(int id);
        Task<BaseResponseDto<List<CategoryDto>>> GetAllCategories();
        Task<BaseResponseDto<CategoryDto>> GetCategory(int id);
    }
}
