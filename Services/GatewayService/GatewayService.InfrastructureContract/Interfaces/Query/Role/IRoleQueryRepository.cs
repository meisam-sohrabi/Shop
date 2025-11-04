using GatewayService.Domain.Entities;

namespace GatewayService.InfrastructureContract.Interfaces.Query.Role
{
    public interface IRoleQueryRepository
    {
        Task<IList<string>> Roles(CustomUserEntity user);

    }
}
