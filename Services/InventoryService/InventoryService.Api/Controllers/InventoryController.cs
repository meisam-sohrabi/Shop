using InventoryService.ApplicationContract.DTO.Base;
using InventoryService.ApplicationContract.DTO.ProductInventory;
using InventoryService.ApplicationContract.Interfaces.ProductInentory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IProductInentoryAppService _productInentoryAppService;

        public InventoryController(IProductInentoryAppService productInentoryAppService)
        {
            _productInentoryAppService = productInentoryAppService;
        }

        [HttpGet("GetById/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<BaseResponseDto<List<ProductInventoryResponseDto>>> GetById([FromRoute] int id)
        {
            return await _productInentoryAppService.GetProductInventory(id);
        }
    }
}
