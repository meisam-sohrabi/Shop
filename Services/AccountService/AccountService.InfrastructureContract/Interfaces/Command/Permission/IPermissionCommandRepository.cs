using AccountService.Domain.Entities;
using AccountService.InfrastructureContract.Interfaces.Command.Generic;

namespace AccountService.InfrastructureContract.Interfaces.Command.Permission
{
    public interface IPermissionCommandRepository : IGenericCommandRepository<PermissionEntity>
    {

    }
}
