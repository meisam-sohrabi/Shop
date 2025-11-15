using ProductService.Domain.Entities;
using ProductService.InfrastructureContract.DapperModel;

namespace ProductService.InfrastructureContract.Interfaces.Query.Product
{
    public interface IProductQueryRespository
    {
        IQueryable<ProductEntity> GetQueryable();
        Task<List<ProductWithInventoryRecord>> GetProductsByDateAndTextAsync(string? textSearch, DateTime? startDate, DateTime? endDate);
    }
}
