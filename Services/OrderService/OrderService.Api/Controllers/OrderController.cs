using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.ApplicationContract.DTO.Base;
using OrderService.ApplicationContract.DTO.Order;
using OrderService.ApplicationContract.Interfaces.Order;

namespace OrderService.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderAppService _orderAppService;

        public OrderController(IOrderAppService orderAppService)
        {
            _orderAppService = orderAppService;
        }
        [HttpPost("PlaceOrder")]
        [Authorize(Roles ="admin")]
        public async Task<BaseResponseDto<OrderResponseDto>> PlaceOrder([FromBody] OrderRequestDto orderRequestDto)
        {
            return await _orderAppService.PlaceOrder(orderRequestDto);
        }
    }
}
