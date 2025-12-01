using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.Services.Attributes;
using ProductService.ApplicationContract.DTO.Base;
using ProductService.ApplicationContract.DTO.Category;
using ProductService.ApplicationContract.Interfaces.Category;
namespace ProductService.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryAppService _categoryAppService;

        public CategoryController(ICategoryAppService categoryAppService)
        {
            _categoryAppService = categoryAppService;
        } 

        [HttpPost("Create")]
        [Authorize(Roles = "admin")]
        [GeneralPermission]
        public async Task<BaseResponseDto<CategoryDto>> Create([FromBody] CategoryDto categoryDto)
        {
            return await _categoryAppService.CreateCategory(categoryDto);

        }

        [HttpPost("Edit/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<BaseResponseDto<CategoryDto>> Edit([FromRoute] int id, [FromBody] CategoryDto categoryDto)
        {
            return await _categoryAppService.EditCategory(id, categoryDto);
        }

        [HttpGet("GetAll")]
        [Authorize]
        public async Task<BaseResponseDto<List<CategoryDto>>> GetAll()
        {
            return await _categoryAppService.GetAllCategories();
        }

        [HttpGet("GetById/{id}")]
        public async Task<BaseResponseDto<CategoryDto>> GetById([FromRoute] int id)
        {
            return await _categoryAppService.GetCategory(id);
        }

        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<BaseResponseDto<CategoryDto>> Delete([FromRoute] int id)
        {
            return await _categoryAppService.DeleteCategory(id);
        }
    }
}
