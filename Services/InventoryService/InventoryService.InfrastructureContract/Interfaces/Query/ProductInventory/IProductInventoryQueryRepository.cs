using InventoryService.Domain.Entities;
using InventoryService.InfrastructureContract.Interfaces.Query.Generic;

namespace InventoryService.InfrastructureContract.Interfaces.Query.ProductInventory
{
    public interface IProductInventoryQueryRepository : IGenericQueryRepository<ProductInventoryEntity>
    {
        // loading...
    }
}
