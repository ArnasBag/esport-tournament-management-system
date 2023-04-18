using AutoMapper;
using ESTMS.API.DataAccess.Entities;
using ESTMS.API.Host.Models;
using Microsoft.AspNetCore.Routing.Constraints;

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
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.ApplicationUser.Email))
            .ReverseMap();

        CreateMap<CreateTeamRequest, Team>();
        CreateMap<Team, TeamResponse>();
        CreateMap<UpdateTeamRequest, Team>();

        CreateMap<Invitation, InvitationResponse>();

        CreateMap<CreatePlayerRequest, Player>();
        CreateMap<UpdatePlayerRequest, Player>();
        CreateMap<Player, PlayerResponse>()
            .ForMember(src => src.Rank, opt => opt.MapFrom(dest => dest.Rank.Value));
    }
}
