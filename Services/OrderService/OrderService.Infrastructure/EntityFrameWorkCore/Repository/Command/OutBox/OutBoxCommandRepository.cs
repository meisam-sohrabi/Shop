using OrderService.Domain.Entities;
using OrderService.Infrastructure.EntityFrameWorkCore.AppDbContext;
using OrderService.Infrastructure.EntityFrameWorkCore.Repository.Command.Generic;
using OrderService.InfrastructureContract.Interfaces.Command.OutBox;

namespace OrderService.Infrastructure.EntityFrameWorkCore.Repository.Command.OutBox
{
    public class OutBoxCommandRepository : GenericCommandRepository<OutBoxMessagesEntity>, IOutBoxCommandRepository
    {
        private readonly ApplicationDbContext _context;

        public OutBoxCommandRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
