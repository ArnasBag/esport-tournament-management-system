using ESTMS.API.Core.Exceptions;
using ESTMS.API.DataAccess.Entities;
using ESTMS.API.DataAccess.Repositories;

namespace ESTMS.API.Services;

public class PlayerScoreService : IPlayerScoreService
{
    private readonly IPlayerScoreRepository _playerScoreRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMatchRepository _matchRepository;

    public PlayerScoreService(IPlayerScoreRepository playerScoreRepository,
        IUserRepository userRepository,
        IMatchRepository matchRepository)
    {
        _playerScoreRepository = playerScoreRepository;
        _userRepository = userRepository;
        _matchRepository = matchRepository;
    }

    public async Task<PlayerScore> CreatePlayerScoreAsync(string userId, int matchId, PlayerScore playerScore)
    {
        var player = await _userRepository.GetPlayerByUserIdAsync(userId)
            ?? throw new NotFoundException("Player with this id was not found.");
        
        var match = await _matchRepository.GetMatchByIdAsync(matchId)
            ?? throw new NotFoundException("Match with this id was not found.");

        if(match.Status == Status.NotStarted)
        {
            throw new BadRequestException("This match has not been started yet.");
        }

        var matchPlayers = match.Competitors.SelectMany(c => c.Players).ToList();

        if(!matchPlayers.Any(p => p.Id == player.Id))
        {
            throw new BadRequestException("This player is not part of this match.");
        }

        if (playerScore.Assists < 0 || playerScore.Deaths < 0 || playerScore.Kills < 0)
        {
            throw new BadRequestException("Cannot create a player score with such values.");
        }

        playerScore.Match = match;
        playerScore.CreatedAt = DateTime.UtcNow;
        await _playerScoreRepository.AssignPlayerScoreToPlayerAsync(player, playerScore);

        return playerScore;
    }

    public Task<List<PlayerScore>> GetPlayerScoresByMatchIdAsync(int matchId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<PlayerScore>> GetPlayerScoresByUserId(string userId)
    {
        var playerScores = await _playerScoreRepository.GetPlayerScoresByUserId(userId);

        return playerScores;
    }
}
