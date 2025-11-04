using ProductService.Domain.Entities;

namespace ProductService.InfrastructureContract.Interfaces.Query.ProductDetail
{
    public interface IProductDetailQueryRepository
    {
        IQueryable<ProductDetailEntity> GetQueryable();
    }
}
