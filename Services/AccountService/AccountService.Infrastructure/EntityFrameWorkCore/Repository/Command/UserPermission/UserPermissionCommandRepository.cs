using AccountService.Domain.Entities;
using AccountService.InfrastructureContract.Interfaces.Command.UserPermission;
using AccountService.Infrastructure.EntityFrameWorkCore.AppDbContext;

namespace AccountService.Infrastructure.EntityFrameWorkCore.Repository.Command.UserPermission
{
    public class UserPermissionCommandRepository : IUserPermissionCommandRepository
    {
        private readonly ApplicationDbContext _context;

        public UserPermissionCommandRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AssignPermissionToUser(UserPermissoinEntity userPermissoinEntity)
        {
            await _context.UserPermissions.AddAsync(userPermissoinEntity);
        }

        public void RevokePermissionFromUser(UserPermissoinEntity userPermissoinEntity)
        {
             _context.UserPermissions.Remove(userPermissoinEntity);
        }
    }
}
