using AccountService.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Infrastructure.EntityFrameWorkCore.Seed
{
    public class DataSeeds
    {
        private readonly UserManager<CustomUserEntity> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public DataSeeds(UserManager<CustomUserEntity> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            if (!await _roleManager.RoleExistsAsync("admin"))
            {
                var adminRole = new IdentityRole { Name = "admin", NormalizedName = "ADMIN" };
                var roleResult = await _roleManager.CreateAsync(adminRole);
            }

            var admin = await _userManager.GetUsersInRoleAsync("admin");
            if (!admin.Any())
            {
                var userName = "admin@yahoo.com";
                var adminUser = await _userManager.Users.FirstOrDefaultAsync(c => c.UserName == userName);

                if (adminUser == null)
                {
                    adminUser = new CustomUserEntity
                    {
                        UserName = userName,
                        FullName = "admin",
                        Email = userName,
                        PhoneNumber = string.Empty  // Replace with actual number if needed
                    };
                    var password = "123456aA!";
                    var userResult = await _userManager.CreateAsync(adminUser, password);
                    var addToRoleResult = await _userManager.AddToRoleAsync(adminUser, "admin");
                }
            }
        }
    }
}