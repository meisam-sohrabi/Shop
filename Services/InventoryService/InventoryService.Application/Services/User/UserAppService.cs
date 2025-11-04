using InventoryService.ApplicationContract;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace InventoryService.Application.Services.User
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
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(userId == null)
            {
                return "36a181c3-5713-4e52-909c-0a99f362b3d7";
            }
            return userId;
        }
    }
}
