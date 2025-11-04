using Microsoft.AspNetCore.Identity;
using AccountService.Domain.Entities;

namespace AccountService.InfrastructureContract.Interfaces.Command.Permission
{
    public interface IPermissionCommandRepository
    {
        void Add(PermissionEntity permissionEntity);
        void Update(PermissionEntity permissionEntity);
        void Delete(PermissionEntity permissionEntity);

    }
}
