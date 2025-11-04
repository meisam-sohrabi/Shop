using GatewayService.Domain.Entities;

namespace GatewayService.InfrastructureContract.Interfaces.Query.Auth
{
    public interface IAuthQueryRrepository
    {
        IQueryable<CustomUserEntity> GetQueryable();
    }
}
