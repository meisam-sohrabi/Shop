using InventoryService.ApplicationContract.DTO.Base;
using InventoryService.ApplicationContract.DTO.ProductInventory;
using InventoryService.ApplicationContract.Interfaces.ProductInentory;
using InventoryService.InfrastructureContract.Interfaces.Query.ProductInventory;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace InventoryService.Application.Services.ProductInentory
{
    public class ProductInventoryAppService : IProductInentoryAppService
    {
        private readonly IProductInventoryQueryRepository _productInventoryQueryRepository;

        public ProductInventoryAppService(IProductInventoryQueryRepository productInventoryQueryRepository)
        {
            _productInventoryQueryRepository = productInventoryQueryRepository;
        }

        #region Get
        public async Task<BaseResponseDto<List<ProductInventoryResponseDto>>> GetProductInventory(int id)
        {
            var output = new BaseResponseDto<List<ProductInventoryResponseDto>>
            {
                Message = "خطا در بازیابی انبار محصول",
                Success = false,
                StatusCode = HttpStatusCode.BadRequest
            };
            var inventory = await _productInventoryQueryRepository.GetQueryable()
                        .Where(c => c.ProductDetailId == id)
                        .Select(c => new ProductInventoryResponseDto { ChangeDate = c.ChangeDate, QuantityChange = c.QuantityChange })
                        .ToListAsync();
            if (inventory == null)
            {
                output.Message = "انبار محصول موردنظر وجود ندارد";
                output.Success = false;
                output.StatusCode = HttpStatusCode.NotFound;
                return output;
            }
            output.Message = "انبار محصول موردنظر با موفقیت دریافت شد";
            output.Success = true;
            output.StatusCode = output.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            output.Data = inventory;
            return output;
        }
        #endregion

    }
}
