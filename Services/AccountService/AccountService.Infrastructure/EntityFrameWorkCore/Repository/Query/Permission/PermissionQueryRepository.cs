using AccountService.Domain.Entities;
using AccountService.InfrastructureContract.Interfaces.Query.Permission;
using AccountService.Infrastructure.EntityFrameWorkCore.AppDbContext;

namespace AccountService.Infrastructure.EntityFrameWorkCore.Repository.Query.Permission
{
    public class PermissionQueryRepository : IPermissionQueryRepository
    {
        private readonly ApplicationDbContext _context;

        public PermissionQueryRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IQueryable<PermissionEntity> GetQueryable()
        {
            return _context.Permissions.AsQueryable();
        }
    }
}
