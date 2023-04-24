using ESTMS.API.DataAccess.Entities;
using Microsoft.AspNetCore.Http;

namespace ESTMS.API.Services;

public interface ITeamService
{
    Task<Team> CreateTeamAsync(Team team, IFormFile logo);
    Task DeactivateTeamAsync(int id);
    Task<Team> UpdateTeamAsync(int id, Team updatedTeam, IFormFile logo);
    Task UpdateTeamPlayersMmrAsync(Team winner, Team loser, int matchId);
    Task<Team> GetTeamByIdAsync(int id);
    Task<Team> GetTeamByTeamManagerId(string teamManagerUserId);
    Task<List<Team>> GetAllTeamsAsync(string? userId = null);
}
