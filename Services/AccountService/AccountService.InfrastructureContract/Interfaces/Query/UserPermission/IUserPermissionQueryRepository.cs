using AccountService.Domain.Entities;

namespace AccountService.InfrastructureContract.Interfaces.Query.UserPermission
{
    public interface IUserPermissionQueryRepository
    {
        IQueryable<UserPermissoinEntity> GetQueryable();
    }
}
