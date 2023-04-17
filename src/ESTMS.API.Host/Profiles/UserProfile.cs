using AutoMapper;
using ESTMS.API.DataAccess.Entities;
using ESTMS.API.Host.Models;

namespace ESTMS.API.Host.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<ApplicationUser, UserResponse>().ReverseMap();
        CreateMap<TeamManager, UserResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ApplicationUser.Id))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.ApplicationUser.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.ApplicationUser.Email));
        CreateMap<Player, UserResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ApplicationUser.Id))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.ApplicationUser.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.ApplicationUser.Email));

        CreateMap<CreateTeamRequest, Team>();
        CreateMap<Team, TeamResponse>();
        CreateMap<UpdateTeamRequest, Team>();

        CreateMap<Invitation, InvitationResponse>();

        CreateMap<CreatePlayerRequest, Player>();
        CreateMap<UpdatePlayerRequest, Player>();
        CreateMap<Player, PlayerResponse>();

        CreateMap<Rank, RankResponse>();
    }
}
