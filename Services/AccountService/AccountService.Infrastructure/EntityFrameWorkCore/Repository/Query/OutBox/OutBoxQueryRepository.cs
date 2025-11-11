using AccountService.Infrastructure.EntityFrameWorkCore.Repository.Query.Generic;
using AccountService.Domain.Entities;
using AccountService.Infrastructure.EntityFrameWorkCore.AppDbContext;
using AccountService.InfrastructureContract.Interfaces.Query.OutBox;

namespace AccountService.Infrastructure.EntityFrameWorkCore.Repository.Query.OutBox
{
    public class OutBoxQueryRepository : GenericQueryRepository<OutBoxMessageEntity>, IOutBoxQueryRepository
    {
        private readonly ApplicationDbContext _context;

        public OutBoxQueryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
