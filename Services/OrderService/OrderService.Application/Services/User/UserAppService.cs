using Microsoft.AspNetCore.Http;
using OrderService.ApplicationContract.Interfaces;
using System.Security.Claims;

namespace OrderService.Application.Services.User
{
    public class UserAppService : IUserAppService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserAppService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string GetCurrentUser()
        {
            if (!_httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false)
            {
                return "36a181c3-5713-4e52-909c-0a99f362b3d7";
            }
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(userId == null)
            {
                return "36a181c3-5713-4e52-909c-0a99f362b3d7";
            }
            return userId;
        }
    }
}
