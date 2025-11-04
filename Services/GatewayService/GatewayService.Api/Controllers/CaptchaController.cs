using GatewayService.ApplicationContract.DTO.Base;
using GatewayService.ApplicationContract.DTO.Token;
using GatewayService.ApplicationContract.Interfaces.Captcha;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GatewayService.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CaptchaController : ControllerBase
    {
        private readonly ICaptchaAppService _captchaAppService;

        public CaptchaController(ICaptchaAppService captchaAppService)
        {
            _captchaAppService = captchaAppService;
        }

        [HttpPost("VerifyCaptcha")]
        public  BaseResponseDto<bool> VerifyCaptcha([FromBody] string code)
        {
            return  _captchaAppService.VerifyCaptcha(code);
        }

        [HttpPost("RefreshCaptcha")]
        public BaseResponseDto<TokenDto> RefreshCaptcha()
        {
            return _captchaAppService.RefreshCaptcha();
        }
    }
}
