using ProductService.Domain.Entities;
using ProductService.InfrastructureContract.Interfaces.Query.Generic;

namespace ProductService.InfrastructureContract.Interfaces.Query.OutBox
{
    public interface IOutBoxQueryRepository : IGenericQueryRepository<OutBoxMessagesEntity>
    {
    }
}
