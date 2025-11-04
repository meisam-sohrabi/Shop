using GatewayService.Domain.Entities;
using GatewayService.Infrastructure.EntityFrameWorkCore.AppDbContext;
using GatewayService.InfrastructureContract.Interfaces.Query.Role;
using Microsoft.EntityFrameworkCore;

namespace GatewayService.Infrastructure.EntityFrameWorkCore.Repository.Query.Role
{
    public class RoleQueryRepository : IRoleQueryRepository
    {
        private readonly ApplicationDbContext _context;

        public RoleQueryRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IList<string>> Roles(CustomUserEntity user)
        {
            var roles = await _context.UserRoles
                .Where(c => c.UserId == user.Id)
                .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => new { roles = r }).Select(c => c.roles.Name).ToListAsync();
            return roles;
        }
    }
}
