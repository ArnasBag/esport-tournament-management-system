using ESTMS.API.DataAccess.Settings;
using ESTMS.API.Services;

namespace ESTMS.API.Host.Capabilities
{
    public static class StartupInjection
    {
        public static IServiceCollection ConfigureInjection(this IServiceCollection services)
        {
            return services
                .AddTransient<IAuthService, AuthService>()
                .AddTransient<ITokenProvider, JwtTokenProvider>();
        }

        public static IServiceCollection ConfigureOptions(this IServiceCollection services, 
            IConfiguration configuration)
        {
            return services
                .Configure<AuthSettings>(configuration.GetSection("Auth"));
        }
    }
}
