using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PricingService.ApplicationContract.DTO.Base;
using PricingService.ApplicationContract.DTO.ProductPrice;
using PricingService.ApplicationContract.Interfaces.ProductPrice;

namespace PricingService.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PriceController : ControllerBase
    {
        private readonly IProductPriceAppService _productPriceAppService;

        public PriceController(IProductPriceAppService productPriceAppService)
        {
            _productPriceAppService = productPriceAppService;
        }

        [HttpPost("SetPrice")]
        [Authorize(Roles = "admin")]
        public async Task<BaseResponseDto<ProductPriceResponseDto>> Create([FromBody]ProductPriceRequestDto priceRequestDto)
        {
           return await _productPriceAppService.CreateProductPrice(priceRequestDto);
        }

        [HttpPost("EditPrice/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<BaseResponseDto<ProductPriceResponseDto>> Edit([FromBody] ProductPriceRequestDto priceRequestDto, [FromRoute] int id)
        {
            return await _productPriceAppService.EditProductPrice(id,priceRequestDto);
        }

        [HttpGet("GetById/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<BaseResponseDto<List<ProductPriceResponseDto>>> GetById([FromRoute] int id)
        {
            return await _productPriceAppService.GetProductPrice(id);
        }
    }
}
