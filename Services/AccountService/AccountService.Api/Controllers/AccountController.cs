using AccountService.ApplicationContract.DTO.Account;
using AccountService.ApplicationContract.DTO.Base;
using AccountService.ApplicationContract.Interfaces.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountAppService _accountAppService;

        public AccountController(IAccountAppService accountAppService)
        {
            _accountAppService = accountAppService;
        }

        [HttpPost("Register")]
        public async Task<BaseResponseDto<ShowUserInfoDto>> Register([FromBody] CreateUserDto createUserDto)
        {
            return await _accountAppService.CreateUser(createUserDto);
        }

        [HttpGet("ShowInfo")]
        [Authorize]
        public async Task<BaseResponseDto<ShowUserInfoDto>> ShowInfo()
        {
            return await _accountAppService.ShowInfo();
        }

        [HttpPost("Edit")]
        [Authorize]
        public async Task<BaseResponseDto<ShowUserInfoDto>> Edit([FromBody] CreateUserDto createUserDto, [FromQuery] string username)
        {
            return await _accountAppService.EditUser(createUserDto,username);
        }

        [HttpDelete("Delete")]
        [Authorize]
        public async Task<BaseResponseDto<ShowUserInfoDto>> Delete(string username)
        {
            return await _accountAppService.DeleteUser(username);
        }
    }
}
