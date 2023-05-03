using ESTMS.API.DataAccess.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.Sqlite;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ESTMS.API.IntegrationTests.Infrastructure;

public class CustomWebApplicationFactory<TProgram> 
    : WebApplicationFactory<TProgram> where TProgram : class
{
    public string DefaultUserId { get; set; } = "1";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.Configure<CustomAuthHandlerOptions>(options => options.DefaultUserId = DefaultUserId);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CustomAuthHandler.AuthenticationScheme;
                options.DefaultScheme = CustomAuthHandler.AuthenticationScheme;
                options.DefaultChallengeScheme = CustomAuthHandler.AuthenticationScheme;
            })
                .AddScheme<CustomAuthHandlerOptions, CustomAuthHandler>(CustomAuthHandler.AuthenticationScheme, options => { });

            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<ApplicationDbContext>));

            services.Remove(dbContextDescriptor);


            // Create open SqliteConnection so EF won't automatically close it.
            services.AddSingleton<DbConnection>(container =>
            {
                var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();

                return connection;
            });

            services.AddDbContext<ApplicationDbContext>((container, options) =>
            {
                var connection = container.GetRequiredService<DbConnection>();
                options.UseSqlite(connection);
                options.ConfigureWarnings(x => x.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.AmbientTransactionWarning));
            });        
        });   
    }
}
