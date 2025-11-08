using OrderService.Domain.Entities;
using OrderService.Infrastructure.EntityFrameWorkCore.AppDbContext;
using OrderService.Infrastructure.EntityFrameWorkCore.Repository.Query.Generic;
using OrderService.InfrastructureContract.Interfaces.Query.OutBox;

namespace OrderService.Infrastructure.EntityFrameWorkCore.Repository.Query.OutBox
{
    public class OutBoxQueryRepository : GenericQueryRepository<OutBoxMessagesEntity>, IOutBoxQueryRepository
    {
        private readonly ApplicationDbContext _context;

        public OutBoxQueryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
