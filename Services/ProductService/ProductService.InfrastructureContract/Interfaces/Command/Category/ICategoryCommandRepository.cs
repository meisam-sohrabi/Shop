using ProductService.InfrastructureContract.Interfaces.Command.Generic;
using ProductService.Domain.Entities;

namespace ProductService.InfrastructureContract.Interfaces.Command.Category
{
    public interface ICategoryCommandRepository : IGenericCommandRepository<CategoryEntity>
    {

        // later implementation here but generic is enough for now.
    }
}
