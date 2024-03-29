using ESTMS.API.DataAccess.Data;
using ESTMS.API.Host.Capabilities;
using ESTMS.API.Host.Middleware;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
});

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(options => options.UseInlineDefinitionsForEnums())
    .ConfigureAuthentication(builder.Configuration)
    .ConfigureAuthorization()
    .ConfigureDatabase(builder.Configuration)
    .ConfigureInjection(builder.Configuration)
    .ConfigureCors()
    .ConfigureOptions(builder.Configuration);

var app = builder.Build();

await SeedData();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("AllowVueFrontend");

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

async Task SeedData()
{
    using var scope = app.Services.CreateScope();
    var seeder = scope.ServiceProvider.GetRequiredService<ApplicationDbSeeder>();
    await seeder.SeedAsync();
}

public partial class Program { }
