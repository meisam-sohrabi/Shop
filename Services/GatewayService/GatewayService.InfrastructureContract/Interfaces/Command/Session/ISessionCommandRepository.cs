using GatewayService.Domain.Entities;

namespace GatewayService.InfrastructureContract.Interfaces.Command.Session
{
    public interface ISessionCommandRepository
    {
        Task Add(UserSessionEntity userSession);
        void Update(UserSessionEntity userSession);
    }
}
