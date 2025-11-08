//using InventoryService.Application.Services.Mapping;
using InventoryService.Application.Services.InventoryConsumer;
using InventoryService.Application.Services.ProductInentory;
using InventoryService.Application.Services.RabbitInventory;
using InventoryService.ApplicationContract.Interfaces.ProductInentory;
using InventoryService.Infrastructure.EntityFrameWorkCore.AppDbContext;
using InventoryService.Infrastructure.EntityFrameWorkCore.Repository.Command.ProductInventory;
using InventoryService.Infrastructure.EntityFrameWorkCore.Repository.Query.ProductInventory;
using InventoryService.Infrastructure.EntityFrameWorkCore.UnitOfWork;
using InventoryService.InfrastructureContract.Interfaces;
using InventoryService.InfrastructureContract.Interfaces.Command.ProductInventory;
using InventoryService.InfrastructureContract.Interfaces.Query.ProductInventory;
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
            services.AddScoped<IProductInventoryQueryRepository, ProductInventoryQueryRepository>();
            services.AddScoped<IProductInentoryAppService,ProductInventoryAppService>();
            services.AddHostedService<InventoryConsumerAppService>();
            services.AddHostedService<OrderConsumerAppService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
