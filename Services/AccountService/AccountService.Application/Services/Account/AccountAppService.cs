using AccountService.ApplicationContract.DTO.Account;
using AccountService.ApplicationContract.DTO.Base;
using AccountService.ApplicationContract.Interfaces;
using AccountService.ApplicationContract.Interfaces.Account;
using AccountService.Domain.Entities;
using AccountService.InfrastructureContract.Interfaces;
using AccountService.InfrastructureContract.Interfaces.Command.Account;
using AccountService.InfrastructureContract.Interfaces.Command.Role;
using AccountService.InfrastructureContract.Interfaces.Query.Account;
using AccountService.InfrastructureContract.Interfaces.Query.Role;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace AccountService.Application.Services.Account
{
    public class AccountAppService : IAccountAppService
    {
        private readonly IAccountCommandRepository _accountCommandRepository;
        private readonly IAccountQueryRepository _accountQueryRepository;
        private readonly IUserAppService _userAppService;
        private readonly IMapper _mapper;
        private readonly IRoleCommandRepository _roleCommandRepository;
        private readonly IRoleQueryRepository _roleQueryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateUserDto> _createUserValidator;

        public AccountAppService(IAccountCommandRepository accountCommandRepository
            , IAccountQueryRepository accountQueryRepository
            , IUserAppService userAppService
            , IMapper mapper
            , IRoleCommandRepository roleCommandRepository
            , IRoleQueryRepository roleQueryRepository,
            IUnitOfWork unitOfWork
            ,IValidator<CreateUserDto> createUserValidator)
        {
            _accountCommandRepository = accountCommandRepository;
            _accountQueryRepository = accountQueryRepository;
            _userAppService = userAppService;
            _mapper = mapper;
            _roleCommandRepository = roleCommandRepository;
            _roleQueryRepository = roleQueryRepository;
            _unitOfWork = unitOfWork;
            _createUserValidator = createUserValidator;
        }

        #region Create
        public async Task<BaseResponseDto<ShowUserInfoDto>> CreateUser(CreateUserDto createUserDto)
        {
            var output = new BaseResponseDto<ShowUserInfoDto>
            {
                Message = "خطا در ایجاد کاربر",
                Success = false,
                StatusCode = HttpStatusCode.BadRequest
            };
            var validationResult = await _createUserValidator.ValidateAsync(createUserDto);
            if (!validationResult.IsValid)
            {
                output.Message = "خطاهای اعتبارسنجی رخ داده است.";
                output.Success = false;
                output.StatusCode = HttpStatusCode.BadRequest;
                output.ValidationErrors = validationResult.ToDictionary();
                return output;
            }
            var identityUser = new CustomUserEntity
            {
                FullName = createUserDto.FullName,
                Email = createUserDto.Email,
                UserName = createUserDto.Email,
                PhoneNumber = createUserDto.PhoneNumber
            };
            var result = await _accountCommandRepository.Create(identityUser, createUserDto.Password);
            if (!result.Succeeded)
            {
                output.Message = "خطا در ایحاد کاربر";
                output.Success = false;
                output.StatusCode = HttpStatusCode.BadRequest;
                return output;
            }
            var roleExist = await _roleQueryRepository.RoleExist("user");
            if (!roleExist)
            {
                var identiyRole = new IdentityRole
                {
                    Name = "user",
                    NormalizedName = "USER"
                };
                await _roleCommandRepository.Add(identiyRole);
               
            }
            await _roleCommandRepository.AssignRoleToUser(identityUser, "user");

            output.Message = "کاربر با موفقبت ایجاد شد";
            output.Success = true;
            output.Data = _mapper.Map<ShowUserInfoDto>(identityUser);

            output.StatusCode = output.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
            return output;
        }

        #endregion

        #region Edit
        public async Task<BaseResponseDto<ShowUserInfoDto>> EditUser(CreateUserDto createUserDto, string username)
        {
            var output = new BaseResponseDto<ShowUserInfoDto>
            {
                Message = "خطا در به روزرسانی کاربر",
                Success = false,
                StatusCode = HttpStatusCode.BadRequest
            };
            var userExist = await _accountQueryRepository.GetQueryable().FirstOrDefaultAsync(c => c.UserName == username);
            if (userExist == null)
            {
                output.Message = "کاربر یافت نشد";
                output.Success = false;
                output.StatusCode = HttpStatusCode.NotFound;
                return output;
            }
            var mapped = _mapper.Map(createUserDto, userExist);
            _accountCommandRepository.Update(mapped);
            var affectedRows = await _unitOfWork.SaveChangesAsync();
            if (affectedRows > 0)
            {
                output.Message = "کاربر با موفقیت به روزرسانی شد";
                output.Success = true;
            }
            output.StatusCode = output.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            return output;
        }
        #endregion

        #region Delete

        // we dont delete a use , we deactive a user by soft delete IsDeleted = true
        public async Task<BaseResponseDto<ShowUserInfoDto>> DeleteUser(string username)
        {
            var output = new BaseResponseDto<ShowUserInfoDto>
            {
                Message = "خطا در به حذف کاربر",
                Success = false,
                StatusCode = HttpStatusCode.BadRequest
            };
            var userExist = await _accountQueryRepository.GetQueryable().FirstOrDefaultAsync(c => c.UserName == username);
            if (userExist == null)
            {
                output.Message = "کاربر یافت نشد";
                output.Success = false;
                output.StatusCode = HttpStatusCode.NotFound;
                return output;
            }
            var affectedRows = await _accountCommandRepository.Delete(userExist);
            if (affectedRows.Succeeded)
            {
                output.Message = "کاربر با موفقیت  حذف شد";
                output.Success = true;
            }
            //var affectedRows = await _unitOfWork.SaveChangesAsync(); // chon identity ma niyaz be savechanges nadarim
            output.StatusCode = output.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            return output;
        }


        #endregion

        #region ShowInfo
        public async Task<BaseResponseDto<ShowUserInfoDto>> ShowInfo()
        {
            var output = new BaseResponseDto<ShowUserInfoDto>
            {
                Message = "خطا در دریافت کاربر",
                Success = false,
                StatusCode = HttpStatusCode.BadRequest
            };
            var userId = _userAppService.GetCurrentUser();
            var user = await _accountQueryRepository.GetQueryable().Where(c => c.Id == userId).Select(c => new ShowUserInfoDto
            {
                FullName = c.FullName,
                PhoneNumber = c.PhoneNumber ?? "No phone number"
            })
                .FirstOrDefaultAsync();
            if (user == null)
            {
                output.Message = "کاربر یافت نشد";
                output.Success = false;
                output.StatusCode = HttpStatusCode.NotFound;
                return output;
            }
            output.Message = "کاربر یافت شد";
            output.Success = true;
            output.Data = user;
            output.StatusCode = output.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            return output;
        }

        #endregion

    }
}
