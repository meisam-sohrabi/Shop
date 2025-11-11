using AccountService.InfrastructureContract.Interfaces.Command.Generic;
using AccountService.Domain.Entities;

namespace AccountService.InfrastructureContract.Interfaces.Command.OutBox
{
    public interface IOutBoxCommandRepository : IGenericCommandRepository<OutBoxMessageEntity>
    {
    }
}
