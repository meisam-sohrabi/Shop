using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AccountService.ApplicationContract.DTO.Base;
using AccountService.ApplicationContract.DTO.Role;
using AccountService.ApplicationContract.Interfaces.Role;
using AccountService.InfrastructureContract.Interfaces.Command.Role;
using AccountService.InfrastructureContract.Interfaces.Query.Account;
using AccountService.InfrastructureContract.Interfaces.Query.Role;
using System.Net;

namespace AccountService.Application.Services.Role
{
    public class RoleAppService : IRoleAppService
    {
        private readonly IRoleCommandRepository _roleCommandRepository;
        private readonly IRoleQueryRepository _roleQueryRepository;
        private readonly IMapper _mapper;
        private readonly IAccountQueryRepository _accountQueryRepository;

        public RoleAppService(IRoleCommandRepository roleCommandRepository
            ,IRoleQueryRepository roleQueryRepository,IMapper mapper,IAccountQueryRepository accountQueryRepository)
        {
            _roleCommandRepository = roleCommandRepository;
            _roleQueryRepository = roleQueryRepository;
            _mapper = mapper;
            _accountQueryRepository = accountQueryRepository;
        }

        #region RevokeRole
        public async Task<BaseResponseDto<RoleDto>> RevokeRole(AssignOrRevokeRoleDto assignOrRevokeRoleDto)
        {
            var output = new BaseResponseDto<RoleDto>
            {
                Message = "خطا در سلب رول از کاربر",
                Success = false,
                StatusCode = HttpStatusCode.BadRequest
            };
            var roleExist = await _roleQueryRepository.GetRoleByName(assignOrRevokeRoleDto.RoleName);
            var userExist = await _accountQueryRepository.GetQueryable().FirstOrDefaultAsync(c => c.Id == assignOrRevokeRoleDto.UserId);
            if (userExist == null)
            {
                output.Message = "یوزر موردنظر یافت نشد";
                output.Success = false;
                output.StatusCode = HttpStatusCode.NotFound;
                return output;
            }
            if (roleExist == null)
            {
                output.Message = "رول موردنظر وجود ندارد";
                output.Success = false;
                output.StatusCode = HttpStatusCode.NotFound;
                return output;
            }
            var result = await _roleCommandRepository.RevokeRoleFromUser(userExist, roleExist.Name);
            if (!result.Succeeded)
            {
                output.Message = "خطا در سلب  رول از کاربر";
                output.Success = false;
                output.StatusCode = HttpStatusCode.BadRequest;
                return output;
            }
            output.Message = "رول با موفقیت از کاربر سلب شد";
            output.Success = true;
            output.StatusCode = output.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            return output;
        }
        #endregion

        #region Assign Role
        public async Task<BaseResponseDto<RoleDto>> AssignRole(AssignOrRevokeRoleDto assignOrRevokeRoleDto)
        {
            var output = new BaseResponseDto<RoleDto>
            {
                Message = "خطا در اختصاص رول به کاربر",
                Success = false,
                StatusCode = HttpStatusCode.BadRequest
            };
            var roleExist = await _roleQueryRepository.GetRoleByName(assignOrRevokeRoleDto.RoleName);
            var userExist = await _accountQueryRepository.GetQueryable().FirstOrDefaultAsync(c => c.Id == assignOrRevokeRoleDto.UserId);
            if (userExist == null)
            {
                output.Message = "یوزر موردنظر یافت نشد";
                output.Success = false;
                output.StatusCode = HttpStatusCode.NotFound;
                return output;
            }
            if (roleExist == null)
            {
                output.Message = "رول موردنظر وجود ندارد";
                output.Success = false;
                output.StatusCode = HttpStatusCode.NotFound;
                return output;
            }
            var result = await _roleCommandRepository.AssignRoleToUser(userExist, roleExist.Name);
            if (!result.Succeeded)
            {
                output.Message = "خطا در اختصاص دادن رول به کاربر";
                output.Success = false;
                output.StatusCode = HttpStatusCode.BadRequest;
                return output;
            }
            output.Message = "رول با موفقیت اختصاص پیدا کرد";
            output.Success = true;
            output.StatusCode = output.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            return output;
        }

        #endregion

        #region Create
        public async Task<BaseResponseDto<RoleDto>> CreateRole(RoleDto roleDto)
        {
            var output = new BaseResponseDto<RoleDto>
            {
                Message = "خطا در ایجاد رول",
                Success = false,
                StatusCode = HttpStatusCode.BadRequest
            };
            var roleExist = await _roleQueryRepository.RoleExist(roleDto.Name);
            if (roleExist)
            {
                output.Message = "رول موردنظر وجود دارد";
                output.Success = false;
                output.StatusCode = HttpStatusCode.Conflict;
                return output;
            }
            var mapped = _mapper.Map<IdentityRole>(roleDto);
            var addRole = await _roleCommandRepository.Add(mapped);
            if (!addRole.Succeeded)
            {
                output.Message = "مشکل در ایجاد رول";
                output.Success = false;
                output.StatusCode = HttpStatusCode.BadRequest;
                return output;
            }
            output.Message = "رول با موفقیت ایجاد شد";
            output.Success = true;
            output.StatusCode = output.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
            return output;
        }
        #endregion

        #region Delete
        public async Task<BaseResponseDto<RoleDto>> DeleteRole(RoleDto roleDto)
        {
            var output = new BaseResponseDto<RoleDto>
            {
                Message = "خطا در دریافت رول",
                Success = false,
                StatusCode = HttpStatusCode.BadRequest
            };
            var role = await _roleQueryRepository.GetRoleByName(roleDto.Name);
            if (role == null)
            {
                output.Message = "رول موردنظر وجود ندارد";
                output.Success = false;
                output.StatusCode = HttpStatusCode.NotFound;
                return output;
            }
            var usersInRole = await _roleQueryRepository.GetUsersInRole(roleDto.Name);
            if (usersInRole.Any())
            {
                output.Message = "به دلیل اختصاص کاربران به این رول، امکان حذف آن وجود ندارد.";
                output.Success = false;
                output.StatusCode = HttpStatusCode.Conflict;
                return output;
            }
            await _roleCommandRepository.Delete(role);
            output.Message = "رول با موفقیت حذف شد";
            output.Success = true;
            output.StatusCode = output.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            return output;
        }
        #endregion

        #region Edit
        public async Task<BaseResponseDto<RoleDto>> EditRole(RoleDto roleDto,string oldRole)
        {
            var output = new BaseResponseDto<RoleDto>
            {
                Message = "خطا در به روزرسانی رول",
                Success = false,
                StatusCode = HttpStatusCode.BadRequest
            };
            var roleExist = await _roleQueryRepository.GetRoleByName(oldRole);
            if (roleExist == null)
            {
                output.Message = "رول موردنظر وجود ندارد";
                output.Success = false;
                output.StatusCode = HttpStatusCode.NotFound;
                return output;
            }
            roleExist.Name = roleDto.Name;
            roleExist.NormalizedName = roleDto.Name.ToUpper();
            var result = await _roleCommandRepository.Update(roleExist);
            if (!result.Succeeded)
            {
                output.Message = "مشکل در به روزرسانی رول";
                output.Success = false;
                output.StatusCode = HttpStatusCode.Conflict;
                return output;
            }
            output.Message = "رول با موفقیت به روزرسانی شد";
            output.Success = true;
            output.StatusCode = output.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            return output;

        }
        #endregion

        #region GetAll
        public async Task<BaseResponseDto<List<RoleDto>>> GetAllRoles()
        {
            var output = new BaseResponseDto<List<RoleDto>>
            {
                Message = "خطا در به دریفات رول ها",
                Success = false,
                StatusCode = HttpStatusCode.BadRequest
            };
            var roles = await _roleQueryRepository.GetAllRoles().Select(c=> new RoleDto { Name = c.Name ?? "There is no name"}).ToListAsync();
            if (!roles.Any())
            {

                output.Message = "هیچ رولی یافت نشد";
                output.Success = false;
                output.StatusCode = HttpStatusCode.Conflict;
                return output;
            }
            output.Message = "رول ها با موفقیت به دریافت  شدند";
            output.Success = true;
            output.Data = roles;
            output.StatusCode = output.Success ? HttpStatusCode.OK : output.StatusCode;
            return output;

        }
        #endregion

    }
}
