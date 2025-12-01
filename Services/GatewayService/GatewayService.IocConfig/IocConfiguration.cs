using FluentValidation;
using GatewayService.Application.Services.Auth;
using GatewayService.Application.Services.Captcha;
using GatewayService.Application.Services.Cookie;
using GatewayService.Application.Services.User;
using GatewayService.ApplicationContract.Interfaces;
using GatewayService.ApplicationContract.Interfaces.Auth;
using GatewayService.ApplicationContract.Interfaces.Captcha;
using GatewayService.ApplicationContract.Validations;
using GatewayService.Domain.Entities;
using GatewayService.Infrastructure.EntityFrameWorkCore.AppDbContext;
using GatewayService.Infrastructure.EntityFrameWorkCore.Repository.Command.Security;
using GatewayService.Infrastructure.EntityFrameWorkCore.Repository.Command.Session;
using GatewayService.Infrastructure.EntityFrameWorkCore.Repository.Query.Auth;
using GatewayService.Infrastructure.EntityFrameWorkCore.Repository.Query.Role;
using GatewayService.Infrastructure.EntityFrameWorkCore.Repository.Query.Security;
using GatewayService.Infrastructure.EntityFrameWorkCore.Repository.Query.Session;
using GatewayService.Infrastructure.EntityFrameWorkCore.UnitOfWork;
using GatewayService.InfrastructureContract.Interfaces;
using GatewayService.InfrastructureContract.Interfaces.Command.Security;
using GatewayService.InfrastructureContract.Interfaces.Command.Session;
using GatewayService.InfrastructureContract.Interfaces.Query.Auth;
using GatewayService.InfrastructureContract.Interfaces.Query.Role;
using GatewayService.InfrastructureContract.Interfaces.Query.Security;
using GatewayService.InfrastructureContract.Interfaces.Query.Session;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace GatewayService.IocConfig
{
    public static class IocConfiguration
    {
        public static IServiceCollection ConfigureIoc(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>();
            services.AddHttpContextAccessor();
            services.AddScoped<IAuthQueryRrepository, AuthQueryRepository>();
            services.AddScoped<IAuthAppService,AuthAppService>();
            services.AddScoped<IPasswordHasher<CustomUserEntity>,PasswordHasher<CustomUserEntity>>();   
            services.AddScoped<IRoleQueryRepository ,RoleQueryRepository>();
            services.AddScoped<ISessionCommandRepository, SessionCommandRepository>();  
            services.AddScoped<ISessionQueryRepository, SessionQueryRepository>();  
            services.AddScoped<IRoleQueryRepository, RoleQueryRepository>();  
            services.AddScoped<ICookieAppService, CookieAppService>();  
            services.AddScoped<IRefreshTokenQueryRepository, RefreshTokenQueryRepository>();  
            services.AddScoped<IRefreshTokenCommandRepository, RefreshTokenCommandRepository>();  
            services.AddScoped<IUserAppService, UserAppService>();  
            services.AddScoped<ICaptchaAppService, CaptchaAppService>();  
            services.AddScoped<IUnitOfWork, UnitOfWork>();


            #region cash-session
            services.AddDistributedMemoryCache();
            services.AddSession(option =>
            {
                option.IdleTimeout = TimeSpan.FromMinutes(5);
                option.Cookie.HttpOnly = true;
                option.Cookie.IsEssential = true;
            });
            #endregion


            #region validation
            services.AddValidatorsFromAssemblyContaining<LoginDtoValidator>();

            #endregion


            return services;
        }
    }
}
