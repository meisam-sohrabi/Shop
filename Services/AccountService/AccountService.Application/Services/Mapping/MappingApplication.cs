using AccountService.ApplicationContract.DTO.Account;
using AccountService.ApplicationContract.DTO.Permission;
using AccountService.ApplicationContract.DTO.Role;
using AccountService.ApplicationContract.DTO.UserPermission;
using AccountService.Domain.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace AccountService.Application.Services.Mapping
{
    public class MappingApplication : Profile
    {
        public MappingApplication()
        {
            CreateMap<CreateUserDto, CustomUserEntity>();
            CreateMap<CustomUserEntity, ShowUserInfoDto>();

            CreateMap<RoleDto, IdentityRole>();
            CreateMap<PermissionEntity, PermissionDto>();
            CreateMap<PermissionDto, PermissionEntity>();
            CreateMap<UserPermissionDto, UserPermissoinEntity>();

        }
    }
}
