using ProductService.InfrastructureContract.Interfaces.Command.Generic;
using ProductService.Domain.Entities;

namespace ProductService.InfrastructureContract.Interfaces.Command.ProductImage
{
    public interface IProductImageCommandRepository : IGenericCommandRepository<ProductImageEntity>
    {
        // other implimentation can be added here also
    }
}
