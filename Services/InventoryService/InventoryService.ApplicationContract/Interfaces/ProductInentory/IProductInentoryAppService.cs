using InventoryService.ApplicationContract.DTO.Base;
using InventoryService.ApplicationContract.DTO.ProductInventory;

namespace InventoryService.ApplicationContract.Interfaces.ProductInentory
{
    public interface IProductInentoryAppService
    {
        Task<BaseResponseDto<List<ProductInventoryResponseDto>>> GetProductInventory(int id);
    }
}
