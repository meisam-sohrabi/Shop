using AccountService.Domain.Entities;
using AccountService.InfrastructureContract.Interfaces.Query.UserPermission;
using AccountService.Infrastructure.EntityFrameWorkCore.AppDbContext;

namespace AccountService.Infrastructure.EntityFrameWorkCore.Repository.Query.UserPermission
{
    public class UserPermissionQueryRepository : IUserPermissionQueryRepository
    {
        private readonly ApplicationDbContext _context;

        public UserPermissionQueryRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IQueryable<UserPermissoinEntity> GetQueryable()
        {
            return _context.UserPermissions.AsQueryable();
        }
    }
}
