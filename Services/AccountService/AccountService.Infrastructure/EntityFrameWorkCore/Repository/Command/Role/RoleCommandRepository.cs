using Microsoft.AspNetCore.Identity;
using AccountService.Domain.Entities;
using AccountService.InfrastructureContract.Interfaces.Command.Role;

namespace AccountService.Infrastructure.EntityFrameWorkCore.Repository.Command.Role
{
    public class RoleCommandRepository : IRoleCommandRepository
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<CustomUserEntity> _userManager;

        public RoleCommandRepository(RoleManager<IdentityRole> roleManager,UserManager<CustomUserEntity> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task<IdentityResult> Add(IdentityRole role)
        {
            return await _roleManager.CreateAsync(role);
        }

        public async Task<IdentityResult> AssignRoleToUser(CustomUserEntity user, string role)
        {
            return await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<IdentityResult> Delete(IdentityRole role)
        {
            return await _roleManager.DeleteAsync(role);
        }

        public async Task<IdentityResult> RevokeRoleFromUser(CustomUserEntity user, string role)
        {
            return await _userManager.RemoveFromRoleAsync(user, role);
        }

        public async Task<IdentityResult> Update(IdentityRole role)
        {
            return await _roleManager.UpdateAsync(role);
        }
    }
}
