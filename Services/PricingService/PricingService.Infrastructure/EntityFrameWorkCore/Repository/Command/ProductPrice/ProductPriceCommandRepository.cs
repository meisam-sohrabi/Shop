using PricingService.Infrastructure.EntityFrameWorkCore.AppDbContext;
using PricingService.Domain.Entities;
using PricingService.InfrastructureContract.Interfaces.Command.ProductPrice;
using PricingService.Infrastructure.EntityFrameWorkCore.Repository.Command.Generic;

namespace PricingService.Infrastructure.EntityFrameWorkCore.Repository.Command.ProductPrice
{
    public class ProductPriceCommandRepository : GenericCommandRepository<ProductPriceEntity>,IProductPriceCommandRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductPriceCommandRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        // loading...
    }
}
