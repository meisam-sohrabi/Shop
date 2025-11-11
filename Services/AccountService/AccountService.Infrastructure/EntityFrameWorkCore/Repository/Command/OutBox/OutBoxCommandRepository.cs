using AccountService.Infrastructure.EntityFrameWorkCore.Repository.Command.Generic;
using AccountService.Domain.Entities;
using AccountService.Infrastructure.EntityFrameWorkCore.AppDbContext;
using AccountService.InfrastructureContract.Interfaces.Command.OutBox;

namespace AccountService.Infrastructure.EntityFrameWorkCore.Repository.Command.OutBox
{
    public class OutBoxCommandRepository : GenericCommandRepository<OutBoxMessageEntity>, IOutBoxCommandRepository
    {
        private readonly ApplicationDbContext _context;

        public OutBoxCommandRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
