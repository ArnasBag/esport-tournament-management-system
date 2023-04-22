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
                .AddTransient<IMatchRepository, MatchRepository>()
                .AddTransient<IPlayerScoreRepository, PlayerScoreRepository>()
                .AddTransient<IMatchService, MatchService>()
                .AddTransient<ITeamManagerRepository, TeamManagerRepository>()
                .AddTransient<IPlayerScoreService, PlayerScoreService>()
                .AddTransient<IInvitationService, InvitationService>()
                .AddTransient<ITeamService, TeamService>()
                .AddTransient<IUserService, UserService>()
                .AddTransient<IPlayerService, PlayerService>()
                .AddTransient<IPlayerRepository, PlayerRepository>()
                .AddTransient<IUserIdProvider, UserIdProvider>()
                .AddTransient<ITournamentService, TournamentService>()
                .AddTransient<ITournamentRepository, TournamentRepository>()
                .AddHttpContextAccessor()
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
