using PricingService.Domain.Entities;
using PricingService.InfrastructureContract.Interfaces.Command.Generic;

namespace PricingService.InfrastructureContract.Interfaces.Command.ProductPrice
{
    public interface IProductPriceCommandRepository : IGenericCommandRepository<ProductPriceEntity>
    {
        // loading...
    }
}
