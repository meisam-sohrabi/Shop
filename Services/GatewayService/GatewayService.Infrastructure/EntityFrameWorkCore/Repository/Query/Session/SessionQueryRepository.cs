using GatewayService.Domain.Entities;
using GatewayService.InfrastructureContract.Interfaces.Query.Session;
using GatewayService.Infrastructure.EntityFrameWorkCore.AppDbContext;

namespace GatewayService.Infrastructure.EntityFrameWorkCore.Repository.Query.Session
{
    public class SessionQueryRepository : ISessionQueryRepository
    {
        private readonly ApplicationDbContext _context;

        public SessionQueryRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public  IQueryable<UserSessionEntity> GetSessionQueryable()
        {
             return _context.UserSessions.AsQueryable();
        }
    }
}
