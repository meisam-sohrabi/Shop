using ProductService.Domain.Entities;
using ProductService.Infrastructure.EntityFrameWorkCore.AppDbContext;
using ProductService.Infrastructure.EntityFrameWorkCore.Repository.Query.Generic;
using ProductService.InfrastructureContract.Interfaces.Query.OutBox;

namespace ProductService.Infrastructure.EntityFrameWorkCore.Repository.Query.OutBox
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
