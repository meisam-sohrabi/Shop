using OrderService.Infrastructure.EntityFrameWorkCore.AppDbContext;
using OrderService.Domain.Entities;
using OrderService.InfrastructureContract.Interfaces.Query.Order;

namespace OrderService.Infrastructure.EntityFrameWorkCore.Repository.Query.Order
{
    public class OrderQueryRepository : IOrderQueryRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderQueryRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IQueryable<OrderEntity> GetQueryable()
        {
           return _context.Orders.AsQueryable();
        }
    }
}
