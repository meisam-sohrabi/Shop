using AccountService.ApplicationContract.DTO.Base;
using AccountService.ApplicationContract.DTO.UserPermission;


namespace AccountService.ApplicationContract.Interfaces.UserPermission
{
    public interface IUserPermissionAppService
    {
        Task<BaseResponseDto<UserPermissionDto>> AssignPermission(UserPermissionDto userPermissionDto);
        Task<BaseResponseDto<UserPermissionDto>> RevokePermission(UserPermissionDto userPermissionDto);
        Task<BaseResponseDto<List<ShowUserPermissionDto>>> GetAllUserPermissions(string userId);
    }
}
