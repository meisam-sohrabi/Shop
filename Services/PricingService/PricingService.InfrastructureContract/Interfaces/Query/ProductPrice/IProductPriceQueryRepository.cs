using PricingService.Domain.Entities;
using PricingService.InfrastructureContract.Interfaces.Query.Generic;

namespace PricingService.InfrastructureContract.Interfaces.Query.ProductPrice
{
    public interface IProductPriceQueryRepository : IGenericQueryRepository<ProductPriceEntity>
    {
        // loading...
    }
}
