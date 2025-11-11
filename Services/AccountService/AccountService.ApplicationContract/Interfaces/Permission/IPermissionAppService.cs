using AccountService.ApplicationContract.DTO.Base;
using AccountService.ApplicationContract.DTO.Permission;
using AccountService.ApplicationContract.DTO.UserPermission;

namespace AccountService.ApplicationContract.Interfaces.Permission
{
    public interface IPermissionAppService
    {
        Task<BaseResponseDto<PermissionDto>> CreatePermission(PermissionDto permissionDto);
        Task<BaseResponseDto<PermissionDto>> EditPermission(int id, PermissionDto permissionDto);
        Task<BaseResponseDto<PermissionDto>> DeletePermission(int id);
        Task<BaseResponseDto<List<ShowAllPermissions>>> GetAllPermissions();
        Task<BaseResponseDto<List<ShowUserPermissionDto>>> GetAllUserNotAssignPermissions(string userId);

    }
}
