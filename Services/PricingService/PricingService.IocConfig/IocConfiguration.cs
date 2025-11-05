using Microsoft.Extensions.DependencyInjection;
using PricingService.Application.Services.Mapping;
using PricingService.Application.Services.ProductPrice;
using PricingService.Application.Services.RabbitPrice;
using PricingService.Application.Services.User;
using PricingService.ApplicationContract.Interfaces;
using PricingService.ApplicationContract.Interfaces.ProductPrice;
using PricingService.Infrastructure.EntityFrameWorkCore.AppDbContext;
using PricingService.Infrastructure.EntityFrameWorkCore.Repository.Command.ProductPrice;
using PricingService.Infrastructure.EntityFrameWorkCore.Repository.Query.ProductPrice;
using PricingService.Infrastructure.EntityFrameWorkCore.UnitOfWork;
using PricingService.InfrastructureContract.Interfaces;
using PricingService.InfrastructureContract.Interfaces.Command.ProductPrice;
using PricingService.InfrastructureContract.Interfaces.Query.ProductPrice;

namespace PricingService.IocConfig
{
    public static class IocConfiguration
    {
        public static IServiceCollection ConfigureIoc(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>();
            services.AddAutoMapper(typeof(MappingApplication).Assembly);
            services.AddScoped<IUserAppService, UserAppService>();
            services.AddScoped<IProductPriceCommandRepository, ProductPriceCommandRepository>();
            services.AddScoped<IProductPriceQueryRepository, ProductPriceQueryRepository>();
            services.AddScoped<IProductPriceAppService, ProductPriceAppService>();
            services.AddHostedService<PriceConsumerAppService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
