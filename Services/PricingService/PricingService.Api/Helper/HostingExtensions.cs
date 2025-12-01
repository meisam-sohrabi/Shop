using BaseConfig;
using MassTransit;

//using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PricingService.Application.Services.PriceConsumer;
using PricingService.IocConfig;
using System.Text;
namespace PricingService.Api.Helper
{
    public static class HostingExtensions
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {


            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
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
                    Title = "PriceServie.Api",
                    Version = "v1",
                    Description = "",
                    Contact = new OpenApiContact
                    {
                        Name = "",
                        Email = ".",
                    },
                });
            });




            //builder.Services.AddStackExchangeRedisCache(option =>
            //{
            //    option.Configuration = "localhost:6379";
            //    option.InstanceName = "";
            //});




            //Stimulsoft.Base.StiLicense.Key = ApplicaitonConfiguration.stiLicense;

            return builder.Build();
        }




        public static WebApplication ConfigurePipelines(this WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
            {
                //app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
            //app.MapHub<ServerConnection>("/printorder");
            app.MapControllers();
            return app;
        }
    }
}
