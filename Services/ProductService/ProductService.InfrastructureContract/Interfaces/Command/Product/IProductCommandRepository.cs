using ProductService.Domain.Entities;

namespace ProductService.InfrastructureContract.Interfaces.Command.Product
{
    public interface IProductCommandRepository
    {
        void Add(ProductEntity product);
        void Edit(ProductEntity product);
        void Delete(ProductEntity product);
        Task EditArabicToPersianSP(DateTime start,DateTime end);
    }
}
