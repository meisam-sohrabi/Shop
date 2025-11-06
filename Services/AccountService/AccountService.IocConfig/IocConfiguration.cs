using AccountService.Application.Services.Account;
using AccountService.Application.Services.Mapping;
using AccountService.Application.Services.Permission;
using AccountService.Application.Services.Role;
using AccountService.Application.Services.User;
using AccountService.Application.Services.UserPermission;
using AccountService.ApplicationContract.Interfaces;
using AccountService.ApplicationContract.Interfaces.Account;
using AccountService.ApplicationContract.Interfaces.Permission;
using AccountService.ApplicationContract.Interfaces.Role;
using AccountService.ApplicationContract.Interfaces.UserPermission;
using AccountService.ApplicationContract.Validations;
using AccountService.Infrastructure.EntityFrameWorkCore.AppDbContext;
using AccountService.Infrastructure.EntityFrameWorkCore.Repository.Command.Account;
using AccountService.Infrastructure.EntityFrameWorkCore.Repository.Command.Permission;
using AccountService.Infrastructure.EntityFrameWorkCore.Repository.Command.Role;
using AccountService.Infrastructure.EntityFrameWorkCore.Repository.Command.UserPermission;
using AccountService.Infrastructure.EntityFrameWorkCore.Repository.Query.Account;
using AccountService.Infrastructure.EntityFrameWorkCore.Repository.Query.Permission;
using AccountService.Infrastructure.EntityFrameWorkCore.Repository.Query.Role;
using AccountService.Infrastructure.EntityFrameWorkCore.Repository.Query.UserPermission;
using AccountService.Infrastructure.EntityFrameWorkCore.Seed;
using AccountService.Infrastructure.EntityFrameWorkCore.UnitOfWork;
using AccountService.InfrastructureContract.Interfaces;
using AccountService.InfrastructureContract.Interfaces.Command.Account;
using AccountService.InfrastructureContract.Interfaces.Command.Permission;
using AccountService.InfrastructureContract.Interfaces.Command.Role;
using AccountService.InfrastructureContract.Interfaces.Command.UserPermission;
using AccountService.InfrastructureContract.Interfaces.Query.Account;
using AccountService.InfrastructureContract.Interfaces.Query.Permission;
using AccountService.InfrastructureContract.Interfaces.Query.Role;
using AccountService.InfrastructureContract.Interfaces.Query.UserPermission;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace AccountService.IocConfig
{
    public static class IocConfiguration
    {
        public static IServiceCollection ConfigureIoc(this IServiceCollection services)
        {

            services.AddDbContext<ApplicationDbContext>();
            services.AddAutoMapper(typeof(MappingApplication).Assembly);
            services.AddScoped<DataSeeds>();
            services.AddScoped<IAccountCommandRepository, AccountCommandRepository>();
            services.AddScoped<IAccountQueryRepository, AccountQueryRepository>();
            services.AddScoped<IRoleCommandRepository, RoleCommandRepository>();
            services.AddScoped<IRoleQueryRepository, RoleQueryRepository>();
            services.AddScoped<IPermissionCommandRepository, PermissionCommandRepository>();
            services.AddScoped<IPermissionQueryRepository, PermissionQueryRepository>();
            services.AddScoped<IUserPermissionCommandRepository, UserPermissionCommandRepository>();
            services.AddScoped<IUserPermissionQueryRepository, UserPermissionQueryRepository>();
            services.AddScoped<IAccountAppService, AccountAppService>();
            services.AddScoped<IUserAppService, UserAppService>();
            services.AddScoped<IRoleAppService, RoleAppService>();
            services.AddScoped<IPermissionAppService, PermissionAppService>();
            services.AddScoped<IUserPermissionAppService, UserPermissionAppService>();
            services.AddScoped<IUnitOfWork,UnitOfWork>();
            services.AddValidatorsFromAssemblyContaining<CreateUserDtoValidator>();
            return services;
        }
    }
}
