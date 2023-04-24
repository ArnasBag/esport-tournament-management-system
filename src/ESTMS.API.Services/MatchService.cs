using ESTMS.API.Core.Exceptions;
using ESTMS.API.DataAccess.Entities;
using ESTMS.API.DataAccess.Repositories;

namespace ESTMS.API.Services;

public class MatchService : IMatchService
{
    private readonly IMatchRepository _matchRepository;
    private readonly IPlayerScoreRepository _playerScoreRepository;

    public MatchService(IMatchRepository matchRepository, 
        IPlayerScoreRepository playerScoreRepository)
    {
        _matchRepository = matchRepository;
        _playerScoreRepository = playerScoreRepository;
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
}
