using ProductService.Domain.Entities;
using ProductService.InfrastructureContract.Interfaces.Query.Generic;

namespace ProductService.InfrastructureContract.Interfaces.Query.ProductDetail
{
    public interface IProductDetailQueryRepository : IGenericQueryRepository<ProductDetailEntity>
    {
    }
}
