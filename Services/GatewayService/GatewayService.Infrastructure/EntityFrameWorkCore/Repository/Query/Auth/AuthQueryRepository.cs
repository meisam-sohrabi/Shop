using GatewayService.Domain.Entities;
using GatewayService.Infrastructure.EntityFrameWorkCore.AppDbContext;
using GatewayService.InfrastructureContract.Interfaces.Query.Auth;

namespace GatewayService.Infrastructure.EntityFrameWorkCore.Repository.Query.Auth
{
    public class AuthQueryRepository : IAuthQueryRrepository
    {
        private readonly ApplicationDbContext _context;

        public AuthQueryRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IQueryable<CustomUserEntity> GetQueryable()
        {
            return _context.Users.AsQueryable();
        }
    }
}
