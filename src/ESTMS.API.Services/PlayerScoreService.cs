using ESTMS.API.Core.Exceptions;
using ESTMS.API.DataAccess.Entities;
using ESTMS.API.DataAccess.Repositories;

namespace ESTMS.API.Services;

public class PlayerScoreService : IPlayerScoreService
{
    private readonly IPlayerScoreRepository _playerScoreRepository;

    public PlayerScoreService(IPlayerScoreRepository playerScoreRepository)
    {
        _playerScoreRepository = playerScoreRepository;
    }

    public async Task<PlayerScore> CreatePlayerScoreAsync(PlayerScore playerScore)
    {
        if(playerScore.Assists < 0 || playerScore.Deaths < 0 || playerScore.Kills < 0)
        {
            throw new BadRequestException("Cannot create a player score with such values.");
        }

        await _playerScoreRepository.CreatePlayerScoreAsync(playerScore);

        return playerScore;
    }

    public Task<List<PlayerScore>> GetPlayerScoresByMatchIdAsync(int matchId)
    {
        throw new NotImplementedException();
    }
}
