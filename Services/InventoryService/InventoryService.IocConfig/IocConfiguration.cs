//using InventoryService.Application.Services.Mapping;
using InventoryService.Infrastructure.EntityFrameWorkCore.AppDbContext;
using InventoryService.Infrastructure.EntityFrameWorkCore.Repository.Command.ProductInventory;
using InventoryService.Infrastructure.EntityFrameWorkCore.UnitOfWork;
using InventoryService.InfrastructureContract.Interfaces;
using InventoryService.InfrastructureContract.Interfaces.Command.ProductInventory;
using Microsoft.Extensions.DependencyInjection;
namespace InventoryService.IocConfig
{
    public static class IocConfiguration
    {
        public static IServiceCollection ConfigureIoc(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>();
            //services.AddAutoMapper(typeof(MappingApplication).Assembly);
            services.AddScoped<IProductInventoryCommandRepository, ProductInventoryCommandRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            //services.AddSingleton<IRabbitPublisherAppService,>();
            return services;
        }
    }
}
