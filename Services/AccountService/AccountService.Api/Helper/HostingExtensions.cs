using AccountService.Domain.Entities;
using AccountService.Infrastructure.EntityFrameWorkCore.AppDbContext;
using AccountService.IocConfig;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using BaseConfig;
namespace AccountService.Api.Helper
{
    public static class HostingExtensions
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddIdentity<CustomUserEntity, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
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
                    ValidateLifetime = true,
                    ValidIssuer = ApplicaitonConfiguration.jwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ApplicaitonConfiguration.jwtKey)),// gets just one key , its dic
                    ValidAudience = ApplicaitonConfiguration.jwtAudience,
                    ClockSkew = TimeSpan.Zero
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
                    Title = "AccountService.Api",
                    Version = "v1",
                    Description = "",
                    Contact = new OpenApiContact
                    {
                        Name = "",
                        Email = ".",
                    },
                });
            });



            return builder.Build();


        }

        public static WebApplication ConfigurePipelines(this WebApplication app)
        {


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
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
