using AccountService.Application.Services.Account;
using AccountService.Application.Services.Job;
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
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace AccountService.IocConfig
{
    public static class IocConfiguration
    {
        public static IServiceCollection ConfigureIoc(this IServiceCollection services)
        {

            services.AddDbContext<ApplicationDbContext>();
            services.AddAutoMapper(typeof(MappingApplication).Assembly);
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
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddValidatorsFromAssemblyContaining<CreateUserDtoValidator>();


            #region MassTransit
            services.AddMassTransit(busConfigurator =>
            {
                busConfigurator.SetKebabCaseEndpointNameFormatter();
                busConfigurator.AddEntityFrameworkOutbox<ApplicationDbContext>(opt =>
                {
                    opt.UseSqlServer();
                    opt.UseBusOutbox();// اگر خواستی استفاده کنی دیگه دستی لازم به ساخت ترانزاکشن نیست
                    opt.QueryDelay = TimeSpan.FromSeconds(5);
                });
                busConfigurator.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                    cfg.ConfigureEndpoints(context);
                });

            });
            #endregion



            #region Quartz
            services.AddQuartz(option =>
            {
                var jobKey = new JobKey("SeedData");
                option.AddJob<SeedDataAppService>(j => j.WithIdentity(jobKey));
                option.AddTrigger(t => t.ForJob(jobKey).WithIdentity("SeedData-trigger")
                .StartNow()
                .WithSimpleSchedule(s => s.WithIntervalInSeconds(30)
                .WithRepeatCount(1)));
            });
            services.AddQuartzHostedService(h =>
            {
                h.WaitForJobsToComplete = true;
            });
            #endregion





            return services;
        }
    }
}
