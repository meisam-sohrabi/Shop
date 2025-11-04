using ProductService.Domain.Entities;

namespace ProductService.InfrastructureContract.Interfaces.Query.ProductBrand
{
    public interface IProductBrandQueryRepository
    {
        IQueryable<ProductBrandEntity> GetQueryable();
    }
}
