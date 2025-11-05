using PricingService.Infrastructure.EntityFrameWorkCore.AppDbContext;
using PricingService.Domain.Entities;
using PricingService.InfrastructureContract.Interfaces.Query.ProductPrice;
using PricingService.Infrastructure.EntityFrameWorkCore.Repository.Query.Generic;

namespace PricingService.Infrastructure.EntityFrameWorkCore.Repository.Query.ProductPrice
{
    public class ProductPriceQueryRepository : GenericQueryRepository<ProductPriceEntity>,IProductPriceQueryRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductPriceQueryRepository(ApplicationDbContext context) : base(context) 
        {
            _context = context;
        }

    }
}
