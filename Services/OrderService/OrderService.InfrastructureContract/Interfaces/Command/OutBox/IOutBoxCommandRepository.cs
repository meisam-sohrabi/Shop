using OrderService.Domain.Entities;
using OrderService.InfrastructureContract.Interfaces.Command.Generic;

namespace OrderService.InfrastructureContract.Interfaces.Command.OutBox
{
    public interface IOutBoxCommandRepository : IGenericCommandRepository<OutBoxMessagesEntity>
    {
    }
}
