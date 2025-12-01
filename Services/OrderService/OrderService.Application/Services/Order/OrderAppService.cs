using BaseConfig;
using FluentValidation;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OrderService.ApplicationContract.DTO.Base;
using OrderService.ApplicationContract.DTO.Order;
using OrderService.ApplicationContract.DTO.ProductDetail;
using OrderService.ApplicationContract.DTO.ProductPriceResponse;
using OrderService.ApplicationContract.Interfaces;
using OrderService.ApplicationContract.Interfaces.Order;
using OrderService.Domain.Entities;
using OrderService.InfrastructureContract.Interfaces;
using OrderService.InfrastructureContract.Interfaces.Command.Order;
using OrderService.InfrastructureContract.Interfaces.Query.Order;
using System.Net;
using System.Net.Http.Headers;
namespace OrderService.Application.Services.Order
{
    public class OrderAppService : IOrderAppService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IUserAppService _userAppService;
        private readonly IOrderCommandRepository _orderCommandRepository;
        private readonly IOrderQueryRepository _orderQueryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<OrderRequestDto> _orderValidator;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrderAppService(
             HttpClient httpClient
            , IHttpContextAccessor httpContext
            , IUserAppService userAppService
            , IOrderCommandRepository orderCommandRepository, IOrderQueryRepository orderQueryRepository
            , IUnitOfWork unitOfWork
            , IValidator<OrderRequestDto> orderValidator
            , IPublishEndpoint publishEndpoint)
        {
            _httpClient = httpClient;
            _httpContext = httpContext;
            _userAppService = userAppService;
            _orderCommandRepository = orderCommandRepository;
            _orderQueryRepository = orderQueryRepository;
            _unitOfWork = unitOfWork;
            _orderValidator = orderValidator;
            _publishEndpoint = publishEndpoint;
        }

        #region PlaceOrder
        public async Task<BaseResponseDto<OrderResponseDto>> PlaceOrder(OrderRequestDto orderRequestDto)
        {
            var output = new BaseResponseDto<OrderResponseDto>
            {
                Message = "خطا در درخواست ",
                Success = false,
                StatusCode = HttpStatusCode.BadRequest
            };
            // در این قسمت ولیدیشن صورت میگیره
            var validationResult = await _orderValidator.ValidateAsync(orderRequestDto);

            // در این قسمت چک میشه و یک دیکشنری که کلید اسم پراپرتی هستش و ولیو لیستی از خطا
            if (!validationResult.IsValid)
            {
                output.Message = "خطاهای اعتبارسنجی رخ داده است.";
                output.Success = false;
                output.StatusCode = HttpStatusCode.BadRequest;
                output.ValidationErrors = validationResult.ToDictionary();
                return output;
            }
            var token = _httpContext.HttpContext.Request.Headers.Authorization.ToString();
            try
            {
                var productDetailAddress = $"https://localhost:7104/ProductDetail/GetById/{orderRequestDto.ProductDetailId}";
                var detailResponse = await GetAsync<ProductDetailResponseDto>(productDetailAddress, token);
                if (detailResponse.Data == null || detailResponse.Data.Quantity < orderRequestDto.Quantity || detailResponse.Data.Quantity == 0)
                {
                    output.Message = detailResponse.Data == null ? "محصول مورد نظر یافت نشد" : "تعداد درخواست بیشتر از موجودی در انبار می باشد";
                    output.StatusCode = detailResponse.StatusCode;
                    output.Success = detailResponse.Success;
                    return output;
                }
                var productPriceAddress = $"https://localhost:7129/Price/GetById/{detailResponse.Data.Id}";
                var priceResponse = await GetAsync<ProductPriceResponseDto>(productPriceAddress, token);
                if (priceResponse.Data == null)
                {
                    output.Message = "قیمتی برای محصول یافت نشد";
                    output.StatusCode = priceResponse.StatusCode;
                    output.Success = priceResponse.Success;
                    return output;
                }
                var setOrder = new OrderEntity
                {
                    Quantity = orderRequestDto.Quantity,
                    TotalPrice = priceResponse.Data.Price * orderRequestDto.Quantity,
                    UserId = _userAppService.GetCurrentUser(),
                    ProductDetailId = detailResponse.Data.Id,
                };

                await _orderCommandRepository.AddAsync(setOrder);

                await _publishEndpoint.Publish(
                    new BaseConfig.ProductDetailEventDto
                    {
                        Id = detailResponse.Data.Id,
                        Quantity = orderRequestDto.Quantity,
                    });

                await _publishEndpoint.Publish(
                    new InventoryEventDto
                    {
                        ProductDetailId = detailResponse.Data.Id,
                        QuantityChange = -orderRequestDto.Quantity,
                        UserId = _userAppService.GetCurrentUser()
                    });
                await _unitOfWork.SaveChangesAsync();
                output.Message = "سفارش  با موفقیت ایجاد شد";
                output.Success = true;
                output.StatusCode = HttpStatusCode.Created;
                output.Data = new OrderResponseDto { Quantity = setOrder.Quantity, TotalPrice = setOrder.TotalPrice, OrderedAt = setOrder.OrderedAt };
                return output;
            }
            catch (HttpRequestException ex)
            {
                output.Message = $"خطای سرور: {ex.Message}";
                output.Success = false;
                output.StatusCode = HttpStatusCode.InternalServerError;
                return output;
            }
            catch (Exception ex)
            {
                output.Message = $"خطای سرور: {ex.Message}";
                output.Success = false;
                output.StatusCode = HttpStatusCode.InternalServerError;
                return output;
            }
        }


        #region AddTokenToHeader
        private void AddBearerToken(HttpRequestMessage request, string token)
        {
            if (token != null && token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                var cleanToken = token.Substring("Bearer ".Length);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", cleanToken);
            }
        }
        #endregion

        #region GetRequests
        private async Task<BaseResponseDto<T>> GetAsync<T>(string address, string token)
        {
            var output = new BaseResponseDto<T>();
            var request = new HttpRequestMessage(HttpMethod.Get, address);
            AddBearerToken(request, token);
            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                output.Message = $"خطای سرور: {response.StatusCode}";
                output.StatusCode = response.StatusCode;
                output.Success = false;
                return output;
            }
            var result = await response.Content.ReadAsStringAsync();
            var Deserilized = JsonConvert.DeserializeObject<BaseResponseDto<T>>(result);
            return Deserilized;

        }
        #endregion

        #endregion


        #region GetUserOrder
        public async Task<BaseResponseDto<List<OrderResponseDto>>> GetAllOrders()
        {
            var output = new BaseResponseDto<List<OrderResponseDto>>
            {
                Message = "خطا در دریافت سفارش",
                Success = false,
                StatusCode = HttpStatusCode.BadRequest
            };
            var orders = await _orderQueryRepository.GetQueryable().Where(c => c.UserId == _userAppService.GetCurrentUser())
                .Select(c => new OrderResponseDto
                {
                    OrderedAt = c.OrderedAt,
                    Quantity = c.Quantity,
                    TotalPrice = c.TotalPrice,
                })
                .ToListAsync();
            if (orders.Any())
            {
                output.Message = "محصولات با موفقیت دریافت شد";
                output.Success = true;
                output.Data = orders;
            }
            output.StatusCode = output.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            return output;
        }
        #endregion


    }
}
