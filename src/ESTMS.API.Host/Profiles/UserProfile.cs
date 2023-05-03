using AutoMapper;
using ESTMS.API.DataAccess.Entities;
using ESTMS.API.Host.Models.Matches;
using ESTMS.API.Host.Models.Player;
using ESTMS.API.Host.Models.Players;
using ESTMS.API.Host.Models.Teams;
using ESTMS.API.Host.Models.Tournament;
using ESTMS.API.Host.Models.Tournaments;
using ESTMS.API.Host.Models.Users;
using TeamResponse = ESTMS.API.Host.Models.Teams.TeamResponse;


namespace ESTMS.API.Host.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<ApplicationUser, UserResponse>().ReverseMap();
        CreateMap<ApplicationUserWithRole, UserResponse>().ReverseMap();
        CreateMap<TeamManager, UserResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ApplicationUser.Id))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.ApplicationUser.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.ApplicationUser.Email))
            .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.ApplicationUser.Active));
        CreateMap<Player, UserResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ApplicationUser.Id))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.ApplicationUser.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.ApplicationUser.Email))
            .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.ApplicationUser.Active))
            .ReverseMap();

        CreateMap<CreateTeamRequest, Team>();
        CreateMap<Team, TeamResponse>();
        CreateMap<UpdateTeamRequest, Team>();

        CreateMap<Invitation, InvitationResponse>();

        CreateMap<CreatePlayerRequest, Player>();
        CreateMap<UpdatePlayerRequest, Player>();

        CreateMap<ApplicationUser, PlayersUserInfo>();
        CreateMap<Player, PlayerResponse>();

        CreateMap<Team, PlayersTeamResponse>()
            .ForMember(dest => dest.TeamManager, opt => opt.MapFrom(src => src.TeamManager));

        CreateMap<TournamentWinner, TournamentWinnerResponse>();
        CreateMap<TeamResponse, ESTMS.API.Host.Models.Tournament.TeamResponse>();
        CreateMap<Team, ESTMS.API.Host.Models.Tournament.TeamResponse>();
        CreateMap<Team, MatchTeamResponse>();
        CreateMap<MatchWinner, MatchWinnerResponse>();
        CreateMap<Tournament, TournamentResponse>();

        CreateMap<CreateTournamentRequest, Tournament>();
        CreateMap<UpdateTournamentRequest, Tournament>();

        CreateMap<CreatePlayerScoreRequest, PlayerScore>().ReverseMap();
        CreateMap<PlayerScore, DailyPlayerScoreResponse>();
        CreateMap<PlayerScore, PlayerScoreResponse>();

        CreateMap<TournamentManager, TournamentManagerResponse>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.ApplicationUser));

        CreateMap<Match, MatchResponse>()
            .ForMember(dest => dest.RoundId, opt => opt.MapFrom(src => src.Round.Id));

        CreateMap<UpdateMatchDateRequest, Match>();

        CreateMap<MatchWinner, MatchWinnerResponse>();

        CreateMap<Round, RoundResponse>()
            .ForMember(dest => dest.TournamentId, opt => opt.MapFrom(src => src.Tournament.Id));
    }
}
