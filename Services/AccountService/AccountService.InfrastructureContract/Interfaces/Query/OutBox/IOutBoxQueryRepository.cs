using AccountService.Domain.Entities;
using AccountService.InfrastructureContract.Interfaces.Query.Generic;

namespace AccountService.InfrastructureContract.Interfaces.Query.OutBox
{
    public interface IOutBoxQueryRepository : IGenericQueryRepository<OutBoxMessageEntity>
    {
    }
}
