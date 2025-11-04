using GatewayService.Domain.Entities;
using GatewayService.Infrastructure.EntityFrameWorkCore.AppDbContext;
using GatewayService.InfrastructureContract.Interfaces.Query.Security;

namespace GatewayService.Infrastructure.EntityFrameWorkCore.Repository.Query.Security
{
    public class RefreshTokenQueryRepository : IRefreshTokenQueryRepository
    {
        private readonly ApplicationDbContext _context;

        public RefreshTokenQueryRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IQueryable<RefreshTokenEntity> GetRefreshTokenQueryable()
        {
            return _context.RefreshToken.AsQueryable();
        }

    }
}
