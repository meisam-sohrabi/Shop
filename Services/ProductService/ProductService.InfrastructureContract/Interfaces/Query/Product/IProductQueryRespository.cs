using ProductService.ApplicationContract.DTO.Product;
using ProductService.Domain.Entities;

namespace ProductService.InfrastructureContract.Interfaces.Query.Product
{
    public interface IProductQueryRespository
    {
        IQueryable<ProductEntity> GetQueryable();
        Task<List<ProductWithInventoryDto>> GetProductsByDateAndTextAsync(string? textSearch, DateTime? startDate, DateTime? endDate);
    }
}
