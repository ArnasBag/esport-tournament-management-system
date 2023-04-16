﻿using AutoMapper;
using ESTMS.API.DataAccess.Entities;
using ESTMS.API.Host.Models;

namespace ESTMS.API.Host.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<ApplicationUser, UserResponse>();
        CreateMap<Player, PlayerResponse>();
        CreateMap<TeamManager, TeamManagerResponse>();


        CreateMap<CreateTeamRequest, Team>();
        CreateMap<Team, TeamResponse>();
        CreateMap<UpdateTeamRequest, Team>();

        CreateMap<Invitation, InvitationResponse>();
    }
}
