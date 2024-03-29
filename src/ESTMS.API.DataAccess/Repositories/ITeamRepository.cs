﻿using ESTMS.API.DataAccess.Entities;

namespace ESTMS.API.DataAccess.Repositories;

public interface ITeamRepository
{
    Task<Team?> GetTeamByIdAsync(int id);
    Task<Team?> GetTeamByTeamManagerUserId(string teamManagerUserId);
    Task<List<Team>> GetAllTeamsAsync();
    Task<Team> CreateTeamAsync(Team team);
    Task<Team> UpdateTeamAsync(Team updatedTeam);
    Task AssignPlayerToTeamAsync(Team team, Player player);
}
