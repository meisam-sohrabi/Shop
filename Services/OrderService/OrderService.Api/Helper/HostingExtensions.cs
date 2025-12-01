using BaseConfig;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OrderService.Application.Services.Order;
using OrderService.ApplicationContract.Interfaces.Order;
using OrderService.IocConfig;
using System.Text;
namespace OrderService.Api.Helper
{
    public static class HostingExtensions
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {


            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddHttpClient<IOrderAppService, OrderAppService>();
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
                    //In = ParameterLocation.Header,
                    //Description = "Please enter JWT token with Bearer prefix, e.g., 'Bearer {token}'",
                    //Name = "Authorization",
                    //Type = SecuritySchemeType.ApiKey,
                    //Scheme = "Bearer"
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter JWT token only (without 'Bearer ' prefix)"
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
                    Title = "OrderService.Api",
                    Version = "v1",
                    Description = "",
                    Contact = new OpenApiContact
                    {
                        Name = "",
                        Email = ".",
                    },
                });
            });



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
            app.MapControllers();
            return app;
        }
    }
}
