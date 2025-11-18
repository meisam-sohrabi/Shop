using AccountService.Domain.Entities;
using AccountService.Infrastructure.EntityFrameWorkCore.AppDbContext;
using AccountService.Infrastructure.EntityFrameWorkCore.Repository.Command.Generic;
using AccountService.InfrastructureContract.Interfaces.Command.Permission;

namespace AccountService.Infrastructure.EntityFrameWorkCore.Repository.Command.Permission
{
    public class PermissionCommandRepository : GenericCommandRepository<PermissionEntity>, IPermissionCommandRepository
    {
        private readonly ApplicationDbContext _context;

        public PermissionCommandRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
