using AutoMapper;
using ESTMS.API.DataAccess.Entities;
using ESTMS.API.Host.Models;
using ESTMS.API.Host.Models.Player;
using ESTMS.API.Host.Models.Tournament;

namespace ESTMS.API.Host.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<ApplicationUser, UserResponse>().ReverseMap();
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


        CreateMap<Tournament, TournamentResponse>()
            .ForMember(dest => dest.Winner, opt => opt.MapFrom(src => src.Winner))
            .ForMember(dest => dest.Teams, opt => opt.MapFrom(src => src.Teams));


        CreateMap<CreateTournamentRequest, Tournament>();
        CreateMap<UpdateTournamentRequest, Tournament>();

        CreateMap<CreatePlayerScoreRequest, PlayerScore>().ReverseMap();
        CreateMap<PlayerScore, PlayerScoreResponse>();
    }
}
