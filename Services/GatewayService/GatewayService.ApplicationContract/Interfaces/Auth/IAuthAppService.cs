using GatewayService.ApplicationContract.DTO.Auth;
using GatewayService.ApplicationContract.DTO.Base;
using GatewayService.ApplicationContract.DTO.Token;

namespace GatewayService.ApplicationContract.Interfaces.Auth
{
    public interface IAuthAppService
    {
        Task<BaseResponseDto<TokenDto>> Login(LoginDto loginDto);
        Task<BaseResponseDto<bool>> Logout();
        Task<BaseResponseDto<TokenDto>> RefreshTokenRequest(RefreshTokenRequestDto refreshToken);
       
    }
}
