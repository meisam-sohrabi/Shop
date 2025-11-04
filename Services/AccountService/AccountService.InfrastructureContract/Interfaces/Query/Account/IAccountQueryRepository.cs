using AccountService.Domain.Entities;

namespace AccountService.InfrastructureContract.Interfaces.Query.Account
{
    public interface IAccountQueryRepository
    {
        IQueryable<CustomUserEntity> GetQueryable();
    }
}
