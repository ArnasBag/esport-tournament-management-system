using ESTMS.API.Core.Exceptions;
using ESTMS.API.DataAccess.Entities;
using ESTMS.API.DataAccess.Repositories;

namespace ESTMS.API.Services;

public class PlayerScoreService : IPlayerScoreService
{
    private readonly IPlayerScoreRepository _playerScoreRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMatchRepository _matchRepository;
    private readonly ITeamRepository _teamRepository;

    public PlayerScoreService(IPlayerScoreRepository playerScoreRepository,
        IUserRepository userRepository,
        IMatchRepository matchRepository,
        ITeamRepository teamRepository)
    {
        _playerScoreRepository = playerScoreRepository;
        _userRepository = userRepository;
        _matchRepository = matchRepository;
        _teamRepository = teamRepository;
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

        //check if player score for this match has been filled already

        playerScore.Match = match;
        playerScore.CreatedAt = DateTime.UtcNow;
        await _playerScoreRepository.AssignPlayerScoreToPlayerAsync(player, playerScore);

        return playerScore;
    }

    public async Task<double> GetPlayerKdaAsync(string userId)
    {
        var player = await _userRepository.GetPlayerByUserIdAsync(userId)
            ?? throw new NotFoundException("Player with this id was not found.");

        var kda = player.Scores.Average(ps => ps.Deaths == 0 ? 
        ps.Kills + ps.Assists : 
        (ps.Kills + ps.Assists) / (double) ps.Deaths);

        return kda;
    }

    public async Task<double> GetTeamKdaAsync(int id)
    {
        var team = await _teamRepository.GetTeamByIdAsync(id)
            ?? throw new NotFoundException("Team with this id was not found.");

        var teamPlayersScores = team.Players.SelectMany(p => p.Scores).ToList();

        if (!teamPlayersScores.Any())
        {
            throw new BadRequestException("This team did not play any games yet.");
        }

        var kda = teamPlayersScores.Average(ps => ps.Deaths == 0 ?
            ps.Kills + ps.Assists :
            (ps.Kills + ps.Assists) / (double)ps.Deaths);

        return kda;
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

    public async Task<List<DailyPlayerScore>> GetPlayerScoresByTeamId(int teamId, DateTime? from, DateTime? to)
    {
        var team = await _teamRepository.GetTeamByIdAsync(teamId) 
            ?? throw new NotFoundException("Team with this id was not found.");

        var playerScores = team.Players
            .SelectMany(player => player.Scores)
            .Where(s => s.CreatedAt >= from && s.CreatedAt <= to)
            .ToList();

        if (!playerScores.Any())
        {
            throw new BadRequestException("This team does not have any scores yet.");
        }

        var playerDailyScores = playerScores
            .GroupBy(s => s.CreatedAt.Date)
            .Select(g => new DailyPlayerScore
            {
                Date = g.Key,
                TotalKills = g.Sum(s => s.Kills),
                TotalAssists = g.Sum(s => s.Assists),
                TotalDeaths = g.Sum(s => s.Deaths),
            })
            .ToList();
        
        return playerDailyScores;
    }
}
