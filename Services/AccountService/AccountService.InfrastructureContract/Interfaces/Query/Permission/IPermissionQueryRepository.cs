using AccountService.Domain.Entities;

namespace AccountService.InfrastructureContract.Interfaces.Query.Permission
{
    public interface IPermissionQueryRepository 
    {
        IQueryable<PermissionEntity> GetQueryable();
    }
}
