using ESTMS.API.DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace ESTMS.API.Host.Capabilities
{
    public static class StartupDatabase
    {
        public static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("AZURE_POSTGRESQL_CONNECTIONSTRING"),
                    options => options.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        }
    }
}