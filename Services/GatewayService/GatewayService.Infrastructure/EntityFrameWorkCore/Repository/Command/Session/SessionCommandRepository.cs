using GatewayService.Domain.Entities;
using GatewayService.InfrastructureContract.Interfaces.Command.Session;
using GatewayService.Infrastructure.EntityFrameWorkCore.AppDbContext;

namespace GatewayService.Infrastructure.EntityFrameWorkCore.Repository.Command.Session
{
    public class SessionCommandRepository : ISessionCommandRepository
    {
        private readonly ApplicationDbContext _context;

        public SessionCommandRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Add(UserSessionEntity userSession)
        {
            await _context.UserSessions.AddAsync(userSession);
        }

        public  void Update(UserSessionEntity userSession)
        {
             _context.UserSessions.Update(userSession);
        }
    }
}
