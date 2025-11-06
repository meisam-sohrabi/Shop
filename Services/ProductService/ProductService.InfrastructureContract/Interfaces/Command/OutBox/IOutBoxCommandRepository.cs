using ProductService.Domain.Entities;
using ProductService.InfrastructureContract.Interfaces.Command.Generic;

namespace ProductService.InfrastructureContract.Interfaces.Command.OutBox
{
    public interface IOutBoxCommandRepository : IGenericCommandRepository<OutBoxMessagesEntity>
    {
    }
}
