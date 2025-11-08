using OrderService.Domain.Entities;
using OrderService.InfrastructureContract.Interfaces.Query.Generic;

namespace OrderService.InfrastructureContract.Interfaces.Query.Order
{
    public interface IOrderQueryRepository : IGenericQueryRepository<OrderEntity>
    {
    }
}
