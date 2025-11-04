using InventoryService.Infrastructure.EntityFrameWorkCore.AppDbContext;
using InventoryService.InfrastructureContract.Interfaces.Command.ProductInventory;
using InventoryService.Domain.Entities;
using InventoryService.Infrastructure.EntityFrameWorkCore.Repository.Command.Generic;

namespace InventoryService.Infrastructure.EntityFrameWorkCore.Repository.Command.ProductInventory
{
    public class ProductInventoryCommandRepository : GenericCommandRepository<ProductInventoryEntity>,IProductInventoryCommandRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductInventoryCommandRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task Add(ProductInventoryEntity productInventory)
        {
          await _context.ProductInventories.AddAsync(productInventory);
        }
       
    }
}
