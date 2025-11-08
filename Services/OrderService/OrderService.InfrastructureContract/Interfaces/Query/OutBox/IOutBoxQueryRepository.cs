using OrderService.Domain.Entities;
using OrderService.InfrastructureContract.Interfaces.Query.Generic;

namespace OrderService.InfrastructureContract.Interfaces.Query.OutBox
{
    public interface IOutBoxQueryRepository : IGenericQueryRepository<OutBoxMessagesEntity>
    {
    }
}
