using ESTMS.API.DataAccess.Repositories;
using ESTMS.API.DataAccess.Settings;
using ESTMS.API.Host.Profiles;
using ESTMS.API.Services;

namespace ESTMS.API.Host.Capabilities
{
    public static class StartupInjection
    {
        public static IServiceCollection ConfigureInjection(this IServiceCollection services)
        {
            return services
                .AddTransient<IAuthService, AuthService>()
                .AddTransient<ITokenProvider, JwtTokenProvider>()
                .AddTransient<IUserRepository, UserRepository>()
                .AddTransient<ITeamRepository, TeamRepository>()
                .AddTransient<IInvitationRepository, InvitationRepository>()
                .AddTransient<IInvitationService, InvitationService>()
                .AddTransient<ITeamService, TeamService>()
                .AddTransient<IUserService, UserService>()
                .AddAutoMapper(typeof(UserProfile).Assembly);
        }

        public static IServiceCollection ConfigureOptions(this IServiceCollection services, 
            IConfiguration configuration)
        {
            return services
                .Configure<AuthSettings>(configuration.GetSection("Auth"));
        }
    }
}
