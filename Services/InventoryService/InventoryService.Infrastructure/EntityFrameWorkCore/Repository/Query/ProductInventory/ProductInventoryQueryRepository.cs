using InventoryService.Domain.Entities;
using InventoryService.Infrastructure.EntityFrameWorkCore.AppDbContext;
using InventoryService.Infrastructure.EntityFrameWorkCore.Repository.Query.Generic;
using InventoryService.InfrastructureContract.Interfaces.Query.ProductInventory;

namespace InventoryService.Infrastructure.EntityFrameWorkCore.Repository.Query.ProductInventory
{
    public class ProductInventoryQueryRepository : GenericQueryRepository<ProductInventoryEntity>, IProductInventoryQueryRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductInventoryQueryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        // loading...
    }
}
