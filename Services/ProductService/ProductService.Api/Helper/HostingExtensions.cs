using BaseConfig;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Quartz;
using Serilog;
using Serilog.Sinks.Elasticsearch;

//using ProductService.Application.Services.Job;
//using ProductService.Application.Services.SignalR;
//using ProductService.Application.Services.Worker;
using ProductService.ApplicationContract.Validators.Category;
using ProductService.IocConfig;
using System.Text;
namespace ProductService.Api.Helper
{
    public static class HostingExtensions
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
                {
                    AutoRegisterTemplate = true,
                    IndexFormat = "ProductService-api-logs-{0:yyyy.MM.dd}"
                })
                .CreateLogger();

            builder.Services.AddControllers();
            //builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSignalR();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpContextAccessor();
            builder.Services.ConfigureIoc();
            builder.Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option =>
            {
                option.TokenValidationParameters = new()
                {
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidIssuer = ApplicaitonConfiguration.jwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ApplicaitonConfiguration.jwtKey)),
                    ValidAudience = ApplicaitonConfiguration.jwtAudience,
                };
            });
            builder.Services.AddSwaggerGen(c =>
            {
                // Add the JWT Authorization header to Swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter JWT token with Bearer prefix, e.g., 'Bearer {token}'",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                 {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ShopService.Api",
                    Version = "v1",
                    Description = "",
                    Contact = new OpenApiContact
                    {
                        Name = "",
                        Email = ".",
                    },
                });
            });

            builder.Host.UseSerilog();
            builder.Services.AddStackExchangeRedisCache(option =>
            {
                option.Configuration = "localhost:6379";
                option.InstanceName = "";
            });

            //builder.Services.AddQuartz(q =>
            //{
            //    var jobKey = new JobKey("product");
            //    q.AddJob<JobsAppService>(j => j.WithIdentity(jobKey));
            //    q.AddTrigger(t => t.ForJob(jobKey).WithIdentity("product-trigger")
            //    .StartNow()
            //    .WithSimpleSchedule(s => s.WithIntervalInMinutes(1)
            //    .RepeatForever()));
            //});
            //builder.Services.AddQuartzHostedService(h =>
            //{
            //    h.WaitForJobsToComplete = true;
            //});

            //builder.Services.AddHostedService<ConsumerWorker>();

            builder.Services.AddValidatorsFromAssemblyContaining<CategoryDtoValidator>();


            //Stimulsoft.Base.StiLicense.Key = ApplicaitonConfiguration.stiLicense;

            return builder.Build();
        }




        public static WebApplication ConfigurePipelines(this WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();
            //app.MapHub<ServerConnection>("/printorder");
            app.MapControllers();
            return app;
        }
    }
}
