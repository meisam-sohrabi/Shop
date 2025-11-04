using GatewayService.ApplicationContract.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace GatewayService.Application.Services.User
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
                return "0";
            }
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(userId == null)
            {
                return "0";
            }
            return userId;
        }
    }
}
