﻿using Azure.Storage.Blobs;
using ESTMS.API.DataAccess.Data;
using ESTMS.API.DataAccess.Repositories;
using ESTMS.API.DataAccess.Settings;
using ESTMS.API.Host.Profiles;
using ESTMS.API.Services.Auth;
using ESTMS.API.Services.Files;
using ESTMS.API.Services.Matches;
using ESTMS.API.Services.Players;
using ESTMS.API.Services.Teams;
using ESTMS.API.Services.Tournaments;
using ESTMS.API.Services.Users;

namespace ESTMS.API.Host.Capabilities
{
    public static class StartupInjection
    {
        public static IServiceCollection ConfigureInjection(this IServiceCollection services, 
            IConfiguration configuration)
        {
            return services
                .AddTransient<IAuthService, AuthService>()
                .AddTransient<ITokenProvider, JwtTokenProvider>()
                .AddTransient<IUserRepository, UserRepository>()
                .AddTransient<ITeamRepository, TeamRepository>()
                .AddTransient<IInvitationRepository, InvitationRepository>()
                .AddTransient<IMatchRepository, MatchRepository>()
                .AddTransient<IPlayerScoreRepository, PlayerScoreRepository>()
                //.AddTransient<IFileUploader, LocalStorageFileUploader>()
                .AddTransient<IFileUploader, BlobStorageUploader>()
                .AddTransient<IRoundService, RoundService>()
                .AddTransient<IRoundRepository, RoundRepository>()
                .AddTransient<IMatchService, MatchService>()
                .AddTransient<ITeamManagerRepository, TeamManagerRepository>()
                .AddTransient<IPlayerScoreService, PlayerScoreService>()
                .AddTransient<IInvitationService, InvitationService>()
                .AddTransient<ITeamService, TeamService>()
                .AddTransient<IUserService, UserService>()
                .AddTransient<IPlayerService, PlayerService>()
                .AddTransient<IPlayerRepository, PlayerRepository>()
                .AddTransient<IUserIdProvider, UserIdProvider>()

                .AddSingleton(x => new BlobServiceClient(configuration.GetConnectionString("AzureBlob")))
                .AddSingleton<IMmrCalculator, EloRatingCalculator>()

                .AddTransient<ITournamentService, TournamentService>()
                .AddTransient<ITournamentRepository, TournamentRepository>()
                .AddTransient<ITournamentManagerRepository, TournamentManagerRepository>()
                .AddHttpContextAccessor()
                .AddAutoMapper(typeof(UserProfile).Assembly)

                .AddScoped<ApplicationDbSeeder>();      
        }

        public static IServiceCollection ConfigureOptions(this IServiceCollection services, 
            IConfiguration configuration)
        {
            return services
                .Configure<AuthSettings>(configuration.GetSection("Auth"))
                .Configure<MmrSettings>(configuration.GetSection("MMR"));
        }
    }
}
