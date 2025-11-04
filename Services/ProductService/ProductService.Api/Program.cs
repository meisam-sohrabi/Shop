using ProductService.Api.Helper;
var builder = WebApplication.CreateBuilder(args);


var app = builder.ConfigureServices().ConfigurePipelines();

app.Run();
