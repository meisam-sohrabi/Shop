using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
//using ProductService.Application.Services.Attributes;
using ProductService.ApplicationContract.DTO.Base;
using ProductService.ApplicationContract.DTO.ProductBrand;
using ProductService.ApplicationContract.Interfaces.ProductBrand;

namespace ProductService.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductBrandController : ControllerBase
    {
        private readonly IProductBrandAppService _productBrandAppService;

        public ProductBrandController(IProductBrandAppService productBrandAppService)
        {
            _productBrandAppService = productBrandAppService;
        }

        [HttpPost("Create")]
        [Authorize(Roles = "admin")]
        //[GeneralPermission(Resource: "ProductBrandController", Action: "Create")]
        public async Task<BaseResponseDto<ProductBrandDto>> Create([FromBody] ProductBrandDto productDto)
        {
            return await _productBrandAppService.CreateProductBrand(productDto);

        }

        [HttpPost("Edit/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<BaseResponseDto<ProductBrandDto>> Edit([FromRoute] int id, [FromBody] ProductBrandDto productDto)
        {
            return await _productBrandAppService.EditProductBrand(id, productDto);
        }

        [HttpGet("GetAll")]
        [Authorize]
        public async Task<BaseResponseDto<List<ProductBrandDto>>> GetAll()
        {
            return await _productBrandAppService.GetAllProductBrands();
        }

        [HttpGet("GetById/{id}")]
        public async Task<BaseResponseDto<ProductBrandDto>> GetById([FromRoute] int id)
        {
            return await _productBrandAppService.GetProductBrand(id);
        }

        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<BaseResponseDto<ProductBrandDto>> Delete([FromRoute] int id)
        {
            return await _productBrandAppService.DeleteProductBrand(id);
        }
    }
}
