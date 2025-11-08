using OrderService.Infrastructure.EntityFrameWorkCore.AppDbContext;
using OrderService.Domain.Entities;
using OrderService.InfrastructureContract.Interfaces.Command.Order;
using OrderService.Infrastructure.EntityFrameWorkCore.Repository.Command.Generic;

namespace OrderService.Infrastructure.EntityFrameWorkCore.Repository.Command.Order
{
    public class OrderCommandRepository : GenericCommandRepository<OrderEntity>,IOrderCommandRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderCommandRepository(ApplicationDbContext context) : base(context) 
        {
            _context = context;
        }

    }
}
