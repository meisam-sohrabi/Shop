using AccountService.ApplicationContract.DTO.Account;
using AccountService.ApplicationContract.DTO.Base;

namespace AccountService.ApplicationContract.Interfaces.Account
{
    public interface IAccountAppService
    {
        Task<BaseResponseDto<ShowUserInfoDto>> ShowInfo();
        Task<BaseResponseDto<ShowUserInfoDto>> CreateUser(CreateUserDto createUserDto);
        Task<BaseResponseDto<ShowUserInfoDto>> EditUser(CreateUserDto createUserDto,string username);
        Task<BaseResponseDto<ShowUserInfoDto>> DeleteUser(string username);
    }
}
