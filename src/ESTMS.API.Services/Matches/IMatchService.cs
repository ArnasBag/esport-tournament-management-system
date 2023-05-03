using ESTMS.API.DataAccess.Entities;

namespace ESTMS.API.Services.Matches;

public interface IMatchService
{
    Task GenerateMatchesAsync();
    Task<Match> UpdateMatchStatusAsync(int matchId, Status matchStatus);
    Task<Match> UpdateMatchWinnerAsync(int matchId, int winnerTeamId);
    Task<List<PlayerScore>> GetMatchPlayerScores(int matchId);
    Task<Match> UpdateMatchDateAsync(int id, Match match);
    Task<Match> GetMatchByIdAsync(int id);
}
