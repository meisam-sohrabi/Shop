using ProductService.Domain.Entities;
using ProductService.Infrastructure.EntityFrameWorkCore.AppDbContext;
using ProductService.Infrastructure.EntityFrameWorkCore.Repository.Command.Generic;
using ProductService.InfrastructureContract.Interfaces.Command.OutBox;

namespace ProductService.Infrastructure.EntityFrameWorkCore.Repository.Command.OutBox
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
