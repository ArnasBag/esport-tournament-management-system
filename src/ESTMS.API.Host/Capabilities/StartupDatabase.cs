using ESTMS.API.DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace ESTMS.API.Host.Capabilities
{
    public static class StartupDatabase
    {
        public static IServiceCollection ConfigureDatabase(this IServiceCollection services)
        {
            return services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql("Host=localhost;Port=5433;Database=estms;Username=root;Password=root",
                    options => options.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        }
    }
}