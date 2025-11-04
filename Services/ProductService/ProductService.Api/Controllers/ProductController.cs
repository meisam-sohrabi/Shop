using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
//using ProductService.Application.Services.PermissionAttribute;
using ProductService.ApplicationContract.DTO.Base;
using ProductService.ApplicationContract.DTO.Product;
using ProductService.ApplicationContract.DTO.Search;
using ProductService.ApplicationContract.DTO.Transaction;
using ProductService.ApplicationContract.Interfaces.Product;

namespace ProductService.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductAppService _productAppService;

        public ProductController(IProductAppService productAppService)
        {
            _productAppService = productAppService;
        }


        [HttpPost("ProductTransaction")]
        [Authorize(Roles = "admin")]
        //[Permission]
        public async Task<BaseResponseDto<ProductTransactionDto>> ProductTransaction([FromBody] ProductTransactionDto productTransactionDto, [FromQuery] int categoryId, [FromQuery] int productBrandId)
        {
            return await _productAppService.ProductTransaction(productTransactionDto, categoryId, productBrandId);
        }

        [HttpPost("Edit/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<BaseResponseDto<ProductResponseDto>> Edit([FromRoute] int id, [FromBody] ProductRequestDto productDto)
        {
            return await _productAppService.EditProduct(id, productDto);
        }

        [HttpGet("GetAll")]
        public async Task<BaseResponseDto<List<ProductResponseDto>>> GetAll()
        {
            return await _productAppService.GetAllProduct();
        }

        [HttpGet("StoredProcedureGetAll")]
        public async Task<BaseResponseDto<List<ProductWithInventoryDto>>> StoreProcedureGetAll([FromQuery] string? search, [FromQuery] DateTime? start, [FromQuery] DateTime? end)
        {
            return await _productAppService.GetProductWithInventory(search,start,end);
        }

        [HttpPost("StoredProcedureEditAToP")]
        public async Task<BaseResponseDto<ProductResponseDto>> StoreProcedureEditAToP([FromBody] ProductArabicToPersianDto productArabicToPersianDto)
        {
            return await _productAppService.EditArabicToPersianSP(productArabicToPersianDto);
        }

        [HttpGet("GetById/{id}")]
        public async Task<BaseResponseDto<ProductResponseDto>> GetById([FromRoute] int id)
        {
            return await _productAppService.GetProduct(id);
        }

        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<BaseResponseDto<ProductResponseDto>> Delete([FromRoute] int id)
        {
            return await _productAppService.DeleteProduct(id);
        }

        [HttpPost("Search")]
        public async Task<BaseResponseDto<List<SearchResponseDto>>> Search([FromBody] SearchRequestDto search)
        {
            return await _productAppService.AdvanceSearchProduct(search);
        }

    }
}
