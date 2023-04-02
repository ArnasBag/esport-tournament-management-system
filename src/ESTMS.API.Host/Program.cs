using ESTMS.API.Host.Capabilities;
using ESTMS.API.Host.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .ConfigureAuthentication(builder.Configuration)
    .ConfigureAuthorization()
    .ConfigureDatabase()
    .ConfigureInjection()
    .ConfigureOptions(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.MapControllers();

app.Run();
