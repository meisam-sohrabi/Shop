using GatewayService.Domain.Entities;

namespace GatewayService.InfrastructureContract.Interfaces.Query.Session
{
    public interface ISessionQueryRepository
    {
         IQueryable<UserSessionEntity> GetSessionQueryable();
    }
}
