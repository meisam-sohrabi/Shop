using AutoMapper;
using Microsoft.EntityFrameworkCore;
using AccountService.ApplicationContract.DTO.Base;
using AccountService.ApplicationContract.DTO.UserPermission;
using AccountService.Domain.Entities;
using AccountService.InfrastructureContract.Interfaces;
using AccountService.InfrastructureContract.Interfaces.Command.UserPermission;
using AccountService.InfrastructureContract.Interfaces.Query.Account;
using AccountService.InfrastructureContract.Interfaces.Query.Permission;
using AccountService.InfrastructureContract.Interfaces.Query.UserPermission;
using System.Net;
using AccountService.ApplicationContract.Interfaces.UserPermission;

namespace AccountService.Application.Services.UserPermission
{
    public class UserPermissionAppService : IUserPermissionAppService
    {
        private readonly IUserPermissionCommandRepository _userPermissionCommanRepository;
        private readonly IPermissionQueryRepository _permissionQueryRepository;
        private readonly IUserPermissionQueryRepository _userPermissionQueryRepository;
        private readonly IAccountQueryRepository _accountQueryRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UserPermissionAppService(IUserPermissionCommandRepository userPermissionCommanRepository
            , IPermissionQueryRepository permissionQueryRepository,
            IUserPermissionQueryRepository userPermissionQueryRepository
            , IAccountQueryRepository accountQueryRepository
            , IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _userPermissionCommanRepository = userPermissionCommanRepository;
            _permissionQueryRepository = permissionQueryRepository;
            _userPermissionQueryRepository = userPermissionQueryRepository;
            _accountQueryRepository = accountQueryRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        #region Assign Permission
        public async Task<BaseResponseDto<UserPermissionDto>> AssignPermission(UserPermissionDto userPermissionDto)
        {
            var output = new BaseResponseDto<UserPermissionDto>
            {
                Message = "خطا در اختصاص پرمیژن به کاربر",
                Success = false,
                StatusCode = HttpStatusCode.BadRequest
            };
            var permissionExist = await _permissionQueryRepository.GetQueryable().AnyAsync(c => c.Id == userPermissionDto.PermissionId);
            var userExist = await _accountQueryRepository.GetQueryable().FirstOrDefaultAsync(c => c.Id == userPermissionDto.UserId);
            if (userExist == null)
            {
                output.Message = "یوزر موردنظر یافت نشد";
                output.Success = false;
                output.StatusCode = HttpStatusCode.NotFound;
                return output;
            }
            if (!permissionExist)
            {
                output.Message = "پرمیژن موردنظر وجود ندارد";
                output.Success = false;
                output.StatusCode = HttpStatusCode.NotFound;
                return output;
            }
            var mapped = _mapper.Map<UserPermissoinEntity>(userPermissionDto);
            await _userPermissionCommanRepository.AssignPermissionToUser(mapped);
            var affectedRows = await _unitOfWork.SaveChangesAsync();
            if (affectedRows > 0)
            {
                output.Message = $"پرمیژن با موفقیت به کاربر اختصاص یافت";
                output.Success = true;
            }
            output.StatusCode = output.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
            return output;
        }
        #endregion

        #region Revoke Permission
        public async Task<BaseResponseDto<UserPermissionDto>> RevokePermission(UserPermissionDto userPermissionDto)
        {
            var output = new BaseResponseDto<UserPermissionDto>
            {
                Message = "خطا در سلب پرمیژن از کاربر",
                Success = false,
                StatusCode = HttpStatusCode.BadRequest
            };
            var permissionExist = await _permissionQueryRepository.GetQueryable().AnyAsync(c => c.Id == userPermissionDto.PermissionId);
            var userExist = await _accountQueryRepository.GetQueryable().FirstOrDefaultAsync(c => c.Id == userPermissionDto.UserId);
            if (userExist == null)
            {
                output.Message = "یوزر موردنظر یافت نشد";
                output.Success = false;
                output.StatusCode = HttpStatusCode.NotFound;
                return output;
            }
            if (!permissionExist)
            {
                output.Message = "پرمیژن موردنظر وجود ندارد";
                output.Success = false;
                output.StatusCode = HttpStatusCode.NotFound;
                return output;
            }
            var mapped = _mapper.Map<UserPermissoinEntity>(userPermissionDto);
            _userPermissionCommanRepository.RevokePermissionFromUser(mapped);
            var affectedRows = await _unitOfWork.SaveChangesAsync();
            if (affectedRows > 0)
            {
                output.Message = $"پرمیژن با موفقیت از کاربر سلب شد";
                output.Success = true;
            }
            output.StatusCode = output.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
            return output;
        }
        #endregion

        #region GetAllUserPermissions
        public async Task<BaseResponseDto<List<ShowUserPermissionDto>>> GetAllUserPermissions(string userId)
        {
            var output = new BaseResponseDto<List<ShowUserPermissionDto>>
            {
                Message = "خطا در دریافت پرمیژن های کاربر",
                Success = false,
                StatusCode = HttpStatusCode.BadRequest
            };
            var userExist = await _accountQueryRepository.GetQueryable().FirstOrDefaultAsync(c => c.Id == userId);
            if (userExist == null)
            {
                output.Message = "یوزر موردنظر یافت نشد";
                output.Success = false;
                output.StatusCode = HttpStatusCode.NotFound;
                return output;
            }
            var userPermission = await _userPermissionQueryRepository.GetQueryable().Where(p => p.UserId == userExist.Id)
                .Select(p => new ShowUserPermissionDto
                {
                    Resource = p.Permission.Resource,
                    Action = p.Permission.Action,
                })
                .ToListAsync();
            if (!userPermission.Any())
            {
                output.Message = "پرمیژنی برای کاربر ثبت نشده است";
                output.Success = false;
                output.StatusCode = HttpStatusCode.NotFound;
                return output;
            }
            output.Message = "پرمیژن های کاربر با موفقیت دریافت شد";
            output.Success = true;
            output.Data = userPermission;
            output.StatusCode = output.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            return output;
        }


        #endregion

    }
}
