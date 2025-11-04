using InventoryService.Domain.Entities;
using InventoryService.InfrastructureContract.Interfaces.Command.Generic;

namespace InventoryService.InfrastructureContract.Interfaces.Command.ProductInventory
{
    public interface IProductInventoryCommandRepository : IGenericCommandRepository<ProductInventoryEntity>
    {
    }
}
