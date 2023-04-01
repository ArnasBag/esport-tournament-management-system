using ESTMS.API.DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace ESTMS.API.Host.Capabilities
{
    public static class StartupDatabase
    {
        public static IServiceCollection ConfigureDatabase(this IServiceCollection services)
        {
            return services.AddDbContext<ApplicationDbContext>(options => 
                options.UseNpgsql("Host=localhost;Database=estms;Username=estms;Password=estms",
                    options => options.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        }
    }
}
