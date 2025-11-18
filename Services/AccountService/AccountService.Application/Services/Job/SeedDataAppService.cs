using AccountService.Domain.Entities;
using AccountService.InfrastructureContract.Interfaces;
using AccountService.InfrastructureContract.Interfaces.Command.Permission;
using AccountService.InfrastructureContract.Interfaces.Command.UserPermission;
using AccountService.InfrastructureContract.Interfaces.Query.Permission;
using AccountService.InfrastructureContract.Interfaces.Query.UserPermission;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace AccountService.Application.Services.Job
{
    public class SeedDataAppService : IJob
    {
        private readonly UserManager<CustomUserEntity> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IPermissionCommandRepository _permissionCommandRepository;
        private readonly IUserPermissionQueryRepository _userPermissionQueryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserPermissionCommandRepository _userPermissionCommandRepository;
        private readonly IPermissionQueryRepository _permissionQueryRepository;

        public SeedDataAppService(UserManager<CustomUserEntity> userManager,
            RoleManager<IdentityRole> roleManager, IPermissionCommandRepository permissionCommandRepository
            , IUserPermissionQueryRepository userPermissionQueryRepository
            , IUnitOfWork unitOfWork
            , IUserPermissionCommandRepository userPermissionCommandRepository
            , IPermissionQueryRepository permissionQueryRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _permissionCommandRepository = permissionCommandRepository;
            _userPermissionQueryRepository = userPermissionQueryRepository;
            _unitOfWork = unitOfWork;
            _userPermissionCommandRepository = userPermissionCommandRepository;
            _permissionQueryRepository = permissionQueryRepository;
        }
        public async Task Execute(IJobExecutionContext context)
        {
          await  SeedAdminInfo();
        }

        private async Task SeedAdminInfo()
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

                    _unitOfWork.BeginTransaction();

                    try
                    {
                        var permissions = new List<PermissionEntity>
                    {
                        new PermissionEntity { Resource = "Category", Action = "Create", Description = "Category Creation Permission" },
                        new PermissionEntity { Resource = "Product", Action = "Create", Description = "Product Creation Permission" }
                    };
                        foreach (var perm in permissions)
                        {
                            var exists = await _permissionQueryRepository.GetQueryable()
                                .AnyAsync(p => p.Resource == perm.Resource && p.Action == perm.Action);

                            if (!exists)
                            {
                                await _permissionCommandRepository.AddAsync(perm);
                            }
                        }

                        await _unitOfWork.SaveChangesAsync();

                        var allPermissions = await _permissionQueryRepository.GetQueryable().ToListAsync();
                        foreach (var permission in allPermissions)
                        {
                            var alreadyAssigned = await _userPermissionQueryRepository
                                .GetQueryable()
                                .AnyAsync(c => c.UserId == adminUser.Id && c.PermissionId == permission.Id);
                            if (!alreadyAssigned)
                            {
                                var userPermission = new UserPermissoinEntity
                                {
                                    UserId = adminUser.Id,
                                    PermissionId = permission.Id,
                                };

                                await _userPermissionCommandRepository.AssignPermissionToUser(userPermission);
                            }
                        }
                        await _unitOfWork.SaveChangesAsync();

                        await _unitOfWork.CommitTransactionAsync();
                    }
                    catch (Exception ex)
                    {
                        {
                            await _unitOfWork.RollBackTransactionAsync();
                            Console.WriteLine($"An error happend:{ex.Message}");
                        }
                    }
                }
            }
        }
    }
}
