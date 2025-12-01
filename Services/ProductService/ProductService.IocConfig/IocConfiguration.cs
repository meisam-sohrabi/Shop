using FluentValidation;
using LogService;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Application.Services.Category;
using ProductService.Application.Services.Mapping;
using ProductService.Application.Services.Product;
using ProductService.Application.Services.ProductBrand;
using ProductService.Application.Services.ProductConsumer;
using ProductService.Application.Services.ProductDetail;
using ProductService.Application.Services.User;
using ProductService.ApplicationContract;
using ProductService.ApplicationContract.Interfaces.Category;
using ProductService.ApplicationContract.Interfaces.Product;
using ProductService.ApplicationContract.Interfaces.ProductBrand;
using ProductService.ApplicationContract.Interfaces.ProductDetail;
using ProductService.ApplicationContract.Validators.Category;
using ProductService.Infrastructure.EntityFrameWorkCore.AppDbContext;
using ProductService.Infrastructure.EntityFrameWorkCore.Repository.Command.Category;
using ProductService.Infrastructure.EntityFrameWorkCore.Repository.Command.Generic;
using ProductService.Infrastructure.EntityFrameWorkCore.Repository.Command.Product;
using ProductService.Infrastructure.EntityFrameWorkCore.Repository.Command.ProductBrand;
using ProductService.Infrastructure.EntityFrameWorkCore.Repository.Command.ProductDetail;
using ProductService.Infrastructure.EntityFrameWorkCore.Repository.Command.ProductImage;
using ProductService.Infrastructure.EntityFrameWorkCore.Repository.Query.Category;
using ProductService.Infrastructure.EntityFrameWorkCore.Repository.Query.Generic;
using ProductService.Infrastructure.EntityFrameWorkCore.Repository.Query.Product;
using ProductService.Infrastructure.EntityFrameWorkCore.Repository.Query.ProductBrand;
using ProductService.Infrastructure.EntityFrameWorkCore.Repository.Query.ProductDetail;
using ProductService.Infrastructure.EntityFrameWorkCore.UnitOfWork;
using ProductService.InfrastructureContract.Interfaces;
using ProductService.InfrastructureContract.Interfaces.Command.Category;
using ProductService.InfrastructureContract.Interfaces.Command.Generic;
using ProductService.InfrastructureContract.Interfaces.Command.Product;
using ProductService.InfrastructureContract.Interfaces.Command.ProductBrand;
using ProductService.InfrastructureContract.Interfaces.Command.ProductDetail;
using ProductService.InfrastructureContract.Interfaces.Command.ProductImage;
using ProductService.InfrastructureContract.Interfaces.Query.Category;
using ProductService.InfrastructureContract.Interfaces.Query.Generic;
using ProductService.InfrastructureContract.Interfaces.Query.Product;
using ProductService.InfrastructureContract.Interfaces.Query.ProductBrand;
using ProductService.InfrastructureContract.Interfaces.Query.ProductDetail;
using RedisService;
namespace ProductService.IocConfig
{
    public static class IocConfiguration
    {
        public static IServiceCollection ConfigureIoc(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>();
            services.AddAutoMapper(typeof(MappingApplication).Assembly);
            services.AddScoped<IUserAppService, UserAppService>();
            services.AddScoped<ICategoryCommandRepository, CategoryCommandRepository>();
            services.AddScoped<ICategoryQueryRepository, CategoryQueryRepository>();
            services.AddScoped<IProductCommandRepository, ProductCommandRepository>();
            services.AddScoped<IProductQueryRespository, ProductQueryRepository>();
            services.AddScoped<IProductDetailCommandRepository, ProductDetailCommandRepository>();
            services.AddScoped<IProductBrandCommandRepository, ProductBrandCommandRepository>();
            services.AddScoped<IProductBrandQueryRepository, ProductBrandQueryRepository>();
            services.AddScoped<IProductDetailQueryRepository, ProductDetailQueryRepository>();
            services.AddScoped<IProductImageCommandRepository, ProductImageCommandRepository>();
            services.AddScoped(typeof(IGenericQueryRepository<>), typeof(GenericQueryRepository<>));
            services.AddScoped(typeof(IGenericCommandRepository<>), typeof(GenericCommandRepository<>));
            //services.AddScoped<ICookieAppService, CookieAppService>();
            services.AddSingleton<ILogAppService, LogAppService>(); // log should be singleton
            services.AddScoped<ICategoryAppService, CategoryAppService>();
            services.AddScoped<IProductAppService, ProductAppService>();
            services.AddScoped<IProductBrandAppService, ProductBrandAppService>();
            services.AddScoped<IProductDetailAppService, ProductDetailAppService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICacheAdapter, DistributedCacheAdapter>();
            //services.AddScoped(typeof(IStimulsoftAppService<>), typeof(StimulsoftAppService<>));


            #region Validation

            services.AddValidatorsFromAssemblyContaining<CategoryDtoValidator>();
            #endregion
        
            
            #region Redis
            services.AddStackExchangeRedisCache(option =>
            {
                option.Configuration = "localhost:6379";
                option.InstanceName = "";
            });
            #endregion



            #region MassTransit
            services.AddMassTransit(BusConfigurator =>
            {
                BusConfigurator.SetKebabCaseEndpointNameFormatter();

                BusConfigurator.AddConsumer<PlaceOrderConsumer>(c => c.ConcurrentMessageLimit = 1);
                BusConfigurator.AddConsumer<AssignPermissionInsertConsumer>(c => c.ConcurrentMessageLimit = 1);
                BusConfigurator.AddConsumer<PermissionInsertConsumer>(c => c.ConcurrentMessageLimit = 1);
                BusConfigurator.AddEntityFrameworkOutbox<ApplicationDbContext>(opt =>
                {
                    opt.UseSqlServer();
                    opt.UseBusOutbox();// اگر خواستی استفاده کنی دیگه دستی لازم به ساخت ترانزاکشن نیست
                    opt.QueryDelay = TimeSpan.FromSeconds(5);
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

            return services;
        }
    }
}
