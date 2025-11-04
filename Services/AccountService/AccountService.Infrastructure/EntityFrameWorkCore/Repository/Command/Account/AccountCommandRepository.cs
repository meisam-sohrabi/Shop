using AccountService.Domain.Entities;
using AccountService.InfrastructureContract.Interfaces.Command.Account;
using AccountService.Infrastructure.EntityFrameWorkCore.AppDbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AccountService.Infrastructure.EntityFrameWorkCore.Repository.Command.Account
{
    public class AccountCommandRepository : IAccountCommandRepository
    {
        private readonly UserManager<CustomUserEntity> _userManager;
        private readonly ApplicationDbContext _context;

        public AccountCommandRepository(UserManager<CustomUserEntity> userManager,ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IdentityResult> Create(CustomUserEntity user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> Delete(CustomUserEntity user)
        {
           return await _userManager.DeleteAsync(user);
        }


        public void Update(CustomUserEntity user)
        {
            var entry = _context.Entry(user); // agar faghat yek kelid bashe ba entity property mishe hamino anjam dad
            var key = _context.Model.FindEntityType(typeof(CustomUserEntity))?.FindPrimaryKey();
            if (key != null)
            {
                foreach (var property in key.Properties)
                {
                    entry.Property(property.Name).IsModified = false;
                }
            }
        }

    }
}
