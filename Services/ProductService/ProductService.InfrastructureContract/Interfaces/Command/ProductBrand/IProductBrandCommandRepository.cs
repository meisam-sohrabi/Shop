using ProductService.Domain.Entities;

namespace ProductService.InfrastructureContract.Interfaces.Command.ProductBrand
{
    public interface IProductBrandCommandRepository
    {
        void Add(ProductBrandEntity productBrand);
        void Edit(ProductBrandEntity productBrand);
        void Delete(ProductBrandEntity productBrand);
    }
}
