using ProductService.Domain.Entities;
using ProductService.InfrastructureContract.Interfaces.Command.Generic;

namespace ProductService.InfrastructureContract.Interfaces.Command.ProductDetail
{
    public interface IProductDetailCommandRepository : IGenericCommandRepository<ProductDetailEntity>
    {

    }
}
