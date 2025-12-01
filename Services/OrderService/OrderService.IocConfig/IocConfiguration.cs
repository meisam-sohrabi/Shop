//using OrderService.Application.Services.Mapping;
using FluentValidation;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Services.User;
using OrderService.ApplicationContract.Interfaces;
using OrderService.ApplicationContract.Validators;
using OrderService.Infrastructure.EntityFrameWorkCore.AppDbContext;
using OrderService.Infrastructure.EntityFrameWorkCore.Repository.Command.Order;
using OrderService.Infrastructure.EntityFrameWorkCore.Repository.Query.Order;
using OrderService.Infrastructure.EntityFrameWorkCore.UnitOfWork;
using OrderService.InfrastructureContract.Interfaces;
using OrderService.InfrastructureContract.Interfaces.Command.Order;
using OrderService.InfrastructureContract.Interfaces.Query.Order;
namespace OrderService.IocConfig
{
    public static class IocConfiguration
    {
        public static IServiceCollection ConfigureIoc(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>();
            //services.AddAutoMapper(typeof(MappingApplication).Assembly);
            services.AddScoped<IUserAppService,UserAppService>();
            services.AddScoped<IOrderCommandRepository, OrderCommandRepository>();
            services.AddScoped<IOrderQueryRepository, OrderQueryRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();


            #region MassTransit
            services.AddMassTransit(BusConfigurator =>
            {
                BusConfigurator.SetKebabCaseEndpointNameFormatter();
                BusConfigurator.AddEntityFrameworkOutbox<ApplicationDbContext>(c =>
                {
                    c.UseSqlServer();
                    c.UseBusOutbox();// اگر خواستی استفاده کنی دیگه دستی لازم به ساخت ترانزاکشن نیست
                    c.QueryDelay = TimeSpan.FromSeconds(5);
                });
                BusConfigurator.UsingRabbitMq((context, cfg) =>
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

            #region Validation
            services.AddValidatorsFromAssemblyContaining<OrderRequestDtoValidator>();

            #endregion


            return services;
        }
    }
}
