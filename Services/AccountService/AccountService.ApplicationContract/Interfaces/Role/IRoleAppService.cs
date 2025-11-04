using AccountService.ApplicationContract.DTO.Role;
using AccountService.ApplicationContract.DTO.Base;

namespace AccountService.ApplicationContract.Interfaces.Role
{
    public interface IRoleAppService
    {
        Task<BaseResponseDto<RoleDto>> CreateRole(RoleDto roleDto);
        Task<BaseResponseDto<RoleDto>> EditRole(RoleDto roleDto,string newRole);
        Task<BaseResponseDto<RoleDto>> DeleteRole(RoleDto roleDto);
        Task<BaseResponseDto<List<RoleDto>>> GetAllRoles();
        Task<BaseResponseDto<RoleDto>> AssignRole(AssignOrRevokeRoleDto assignOrRevokeRoleDto);
        Task<BaseResponseDto<RoleDto>> RevokeRole(AssignOrRevokeRoleDto assignOrRevokeRoleDto);
        //Task<BaseResponseDto<RoleDto>> GeteRole(int id);
        //Task<BaseResponseDto<RoleDto>> GetAllRole(RoleDto roleDto);
    }
}
