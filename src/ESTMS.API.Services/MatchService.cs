using ESTMS.API.Core.Exceptions;
using ESTMS.API.DataAccess.Entities;
using ESTMS.API.DataAccess.Repositories;

namespace ESTMS.API.Services;

public class MatchService : IMatchService
{
    private readonly IMatchRepository _matchRepository;
    private readonly IPlayerScoreRepository _playerScoreRepository;
    private readonly ITeamRepository _teamRepository;

    public MatchService(IMatchRepository matchRepository,
        IPlayerScoreRepository playerScoreRepository,
        ITeamRepository teamRepository)
    {
        _matchRepository = matchRepository;
        _playerScoreRepository = playerScoreRepository;
        _teamRepository = teamRepository;
    }

    public Task GenerateMatchesAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Match> UpdateMatchStatusAsync(int matchId, Status matchStatus)
    {
        var match = await _matchRepository.GetMatchByIdAsync(matchId)
            ?? throw new NotFoundException("Match with this id was not found.");

        if(matchStatus == Status.Done)
        {
            var playerScores = await _playerScoreRepository.GetPlayerScoresByMatchIdAsync(matchId);
            var matchParticipants = match.Competitors.SelectMany(c => c.Players);

            if (playerScores.Count != matchParticipants.Count())
            {
                throw new BadRequestException("You must fill all player scores before ending the match");
            }
        }

        match.Status = matchStatus;

        await _matchRepository.UpdateMatchAsync(match);
        return match;
    }

    public async Task<Match> UpdateMatchWinnerAsync(int matchId, int winnerTeamId)
    {
        var match = await _matchRepository.GetMatchByIdAsync(matchId)
            ?? throw new NotFoundException("Match with this id was not found.");

        var team = await _teamRepository.GetTeamByIdAsync(winnerTeamId)
            ?? throw new NotFoundException("Team with this id was not found.");

        if(!match.Competitors.Any(c => c.Id == team.Id))
        {
            throw new BadRequestException("This team did not play in this match.");
        }

        if(match.Winner != null)
        {
            throw new BadRequestException("This match already has a winner");
        }

        match.Winner = new MatchWinner
        {
            Match = match,
            WinnerTeam = team,
        };

        await _matchRepository.UpdateMatchAsync(match);
    }
}
