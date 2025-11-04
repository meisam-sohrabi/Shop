using ProductService.InfrastructureContract.Interfaces.Query.Generic;
using ProductService.Domain.Entities;

namespace ProductService.InfrastructureContract.Interfaces.Query.Category
{
    public interface ICategoryQueryRepository : IGenericQueryRepository<CategoryEntity>
    {
    }
}
