using Microsoft.AspNetCore.Identity;
using AccountService.Domain.Entities;
using AccountService.InfrastructureContract.Interfaces.Query.Account;

namespace AccountService.Infrastructure.EntityFrameWorkCore.Repository.Query.Account
{
    public class AccountQueryRepository : IAccountQueryRepository
    {
        private readonly UserManager<CustomUserEntity> _userManager;

        public AccountQueryRepository(UserManager<CustomUserEntity> userManager)
        {
            _userManager = userManager;
        }


        public  IQueryable<CustomUserEntity> GetQueryable()
        {
            return  _userManager.Users.AsQueryable();
        }

    }
}
