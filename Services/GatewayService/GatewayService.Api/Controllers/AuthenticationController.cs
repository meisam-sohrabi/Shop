using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GatewayService.ApplicationContract.DTO.Auth;
using GatewayService.ApplicationContract.DTO.Base;
using GatewayService.ApplicationContract.Interfaces.Auth;
using GatewayService.ApplicationContract.DTO.Token;

namespace GatewayService.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthAppService _authAppService;

        public AuthenticationController(IAuthAppService authAppService)
        {
            _authAppService = authAppService;
        }

        [HttpPost("Login")]
        public async Task<BaseResponseDto<TokenDto>> Login([FromBody] LoginDto loginDto)
        {
            return await _authAppService.Login(loginDto);
        }

        [HttpPost("Refresh")]
        public async Task<BaseResponseDto<TokenDto>> RefreshToken([FromBody] RefreshTokenRequestDto refreshToken)
        {
            return await _authAppService.RefreshTokenRequest(refreshToken);
        }

        [HttpGet("Logout")]
        [Authorize]
        public async Task<BaseResponseDto<bool>> Logout()
        {
            return await _authAppService.Logout();
        }
    }
}
