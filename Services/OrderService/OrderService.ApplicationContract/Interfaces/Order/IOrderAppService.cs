using OrderService.ApplicationContract.DTO.Base;
using OrderService.ApplicationContract.DTO.Order;


namespace OrderService.ApplicationContract.Interfaces.Order
{
    public interface IOrderAppService
    {
        Task<BaseResponseDto<OrderResponseDto>> PlaceOrder(OrderRequestDto orderRequestDto);
        Task<BaseResponseDto<List<OrderResponseDto>>> GetAllOrders();
    }
}
