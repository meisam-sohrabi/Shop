using Microsoft.AspNetCore.Identity;
using AccountService.Domain.Entities;
using AccountService.InfrastructureContract.Interfaces.Query.Role;

namespace AccountService.Infrastructure.EntityFrameWorkCore.Repository.Query.Role
{
    public class RoleQueryRepository : IRoleQueryRepository
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<CustomUserEntity> _userManager;

        public RoleQueryRepository(RoleManager<IdentityRole> roleManager, UserManager<CustomUserEntity> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IQueryable<IdentityRole> GetAllRoles()
        {
            return _roleManager.Roles;
        }

        public async Task<IdentityRole> GetRoleByName(string role)
        {
            return await _roleManager.FindByNameAsync(role);
        }

        public async Task<IList<CustomUserEntity>> GetUsersInRole(string role)
        {
            return await _userManager.GetUsersInRoleAsync(role);
        }

        public async Task<bool> RoleExist(string role)
        {
            return await _roleManager.RoleExistsAsync(role);
        }

        public async Task<IList<string>> Roles(CustomUserEntity user)
        {
            return await _userManager.GetRolesAsync(user);
        }
    }
}
