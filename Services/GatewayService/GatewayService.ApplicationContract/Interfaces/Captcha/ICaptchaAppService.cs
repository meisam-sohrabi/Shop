using GatewayService.ApplicationContract.DTO.Base;
using GatewayService.ApplicationContract.DTO.Captcha;
using GatewayService.ApplicationContract.DTO.Token;

namespace GatewayService.ApplicationContract.Interfaces.Captcha
{
    public interface ICaptchaAppService
    {
        CaptchaResponseDto GenerateCaptcha(int length = 6);
        BaseResponseDto<bool> VerifyCaptcha(string code);
        BaseResponseDto<TokenDto> RefreshCaptcha();
    }
}
