//using OrderService.Application.Services.Mapping;
using OrderService.Infrastructure.EntityFrameWorkCore.AppDbContext;
using OrderService.Infrastructure.EntityFrameWorkCore.UnitOfWork;
using OrderService.InfrastructureContract.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using OrderService.InfrastructureContract.Interfaces.Command.Order;
using OrderService.Infrastructure.EntityFrameWorkCore.Repository.Command.Order;
using OrderService.InfrastructureContract.Interfaces.Query.Order;
using OrderService.Infrastructure.EntityFrameWorkCore.Repository.Query.Order;
using OrderService.InfrastructureContract.Interfaces.Command.OutBox;
using OrderService.Infrastructure.EntityFrameWorkCore.Repository.Command.OutBox;
using OrderService.InfrastructureContract.Interfaces.Query.OutBox;
using OrderService.Infrastructure.EntityFrameWorkCore.Repository.Query.OutBox;
using OrderService.ApplicationContract.Interfaces;
using OrderService.Application.Services.User;
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
            services.AddScoped<IOutBoxCommandRepository, OutBoxCommandRepository>();
            services.AddScoped<IOutBoxQueryRepository, OutBoxQueryRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
