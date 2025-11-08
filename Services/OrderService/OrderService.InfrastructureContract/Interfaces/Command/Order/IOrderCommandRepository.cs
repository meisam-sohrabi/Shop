using OrderService.Domain.Entities;
using OrderService.InfrastructureContract.Interfaces.Command.Generic;

namespace OrderService.InfrastructureContract.Interfaces.Command.Order
{
    public interface IOrderCommandRepository : IGenericCommandRepository<OrderEntity>
    {
    }
}
