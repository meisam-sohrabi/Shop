using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AccountService.ApplicationContract.DTO.Base;
using AccountService.ApplicationContract.DTO.Role;
using AccountService.ApplicationContract.Interfaces.Role;

namespace AccountService.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleAppService _roleAppService;

        public RoleController(IRoleAppService roleAppService)
        {
            _roleAppService = roleAppService;
        }

        [HttpPost("Create")]
        [Authorize(Roles = "admin")]
        public async Task<BaseResponseDto<RoleDto>> Create([FromBody] RoleDto RoleDto)
        {
            return await _roleAppService.CreateRole(RoleDto);

        }

        [HttpPost("Edit")]
        [Authorize(Roles = "admin")]
        public async Task<BaseResponseDto<RoleDto>> Edit([FromBody] RoleDto RoleDto,[FromQuery] string oldRole)
        {
            return await _roleAppService.EditRole(RoleDto, oldRole);
        }

        [HttpGet("GetAll")]
        [Authorize]
        public async Task<BaseResponseDto<List<RoleDto>>> GetAll()
        {
            return await _roleAppService.GetAllRoles();
        }

        [HttpPost("AssignRole")]
        public async Task<BaseResponseDto<RoleDto>> AssignRole([FromBody] AssignOrRevokeRoleDto assignOrRevokeRoleDto)
        {
            return await _roleAppService.AssignRole(assignOrRevokeRoleDto);
        }

        [HttpPost("RevokeRole")]
        public async Task<BaseResponseDto<RoleDto>> RevokeRole([FromBody] AssignOrRevokeRoleDto assignOrRevokeRoleDto)
        {
            return await _roleAppService.RevokeRole(assignOrRevokeRoleDto);
        }

        [HttpDelete("Delete")]
        [Authorize(Roles = "admin")]
        public async Task<BaseResponseDto<RoleDto>> Delete([FromQuery] RoleDto roleDto)
        {
            return await _roleAppService.DeleteRole(roleDto);
        }
    }
}
