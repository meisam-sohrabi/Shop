using GatewayService.Domain.Entities;

namespace GatewayService.InfrastructureContract.Interfaces.Query.Security
{
    public interface IRefreshTokenQueryRepository
    {
        IQueryable<RefreshTokenEntity> GetRefreshTokenQueryable();
    }
}
