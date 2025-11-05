using AccountService.Infrastructure.EntityFrameWorkCore.Seed;
using AccountService.Api.Helper;

var builder = WebApplication.CreateBuilder(args);


var app = builder.ConfigureServices().ConfigurePipelines();


// creating scope to seed admin data in startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dataSeeder = services.GetRequiredService<DataSeeds>();
    await dataSeeder.SeedAsync();
}

app.Run();
