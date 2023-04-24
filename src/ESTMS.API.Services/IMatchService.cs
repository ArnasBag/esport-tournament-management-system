using ESTMS.API.DataAccess.Entities;

namespace ESTMS.API.Services;

public interface IMatchService
{
    Task GenerateMatchesAsync();
    Task<Match> UpdateMatchStatusAsync(int matchId, Status matchStatus);
    Task<Match> UpdateMatchWinnerAsync(int matchId, int winnerTeamId);
}
