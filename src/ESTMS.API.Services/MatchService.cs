using ESTMS.API.Core.Exceptions;
using ESTMS.API.DataAccess.Entities;
using ESTMS.API.DataAccess.Repositories;

namespace ESTMS.API.Services;

public class MatchService : IMatchService
{
    private readonly IMatchRepository _matchRepository;
    private readonly IPlayerScoreRepository _playerScoreRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly IMmrCalculator _mmrCalculator;
    private readonly IPlayerRepository _playerRepository;
    private readonly ITournamentService _tournamentService;

    public MatchService(IMatchRepository matchRepository,
        IPlayerScoreRepository playerScoreRepository,
        ITeamRepository teamRepository,
        IMmrCalculator mmrCalculator,
        IPlayerRepository playerRepository,
        ITournamentService tournamentService)
    {
        _matchRepository = matchRepository;
        _playerScoreRepository = playerScoreRepository;
        _teamRepository = teamRepository;
        _mmrCalculator = mmrCalculator;
        _playerRepository = playerRepository;
        _tournamentService = tournamentService;
    }

    public Task GenerateMatchesAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Match> UpdateMatchStatusAsync(int matchId, Status matchStatus)
    {
        var match = await _matchRepository.GetMatchByIdAsync(matchId)
                    ?? throw new NotFoundException("Match with this id was not found.");

        if (matchStatus == Status.Done)
        {
            var playerScores = await _playerScoreRepository.GetPlayerScoresByMatchIdAsync(matchId);
            var matchParticipants = match.Competitors.SelectMany(c => c.Players);

            if (playerScores.Count != matchParticipants.Count())
            {
                throw new BadRequestException("You must fill all player scores before ending the match.");
            }

            if (match.Winner is null)
            {
                throw new BadRequestException("You must assign winner team before ending the match.");
            }

            var winnerTeam = await _teamRepository.GetTeamByIdAsync(match.Winner.WinnerTeamId);
            var losingTeam = await _teamRepository.GetTeamByIdAsync(
                match.Competitors.SingleOrDefault(c => c.Id != winnerTeam!.Id)!.Id);

            var losingTeamMmr = (int)losingTeam!.Players.Average(p => p.Mmr);
            var winningTeamMmr = (int)winnerTeam!.Players.Average(p => p.Mmr);

            foreach (var player in winnerTeam!.Players)
            {
                var playersScores = player.Scores;
                var matchPlayerScore = player.Scores.Single(s => s.Match.Id == match.Id);
                player.Mmr = _mmrCalculator.RecalculatePlayerMmr(player, losingTeamMmr, matchPlayerScore, 1);
                await _playerRepository.UpdatePlayerAsync(player);
            }

            foreach (var player in losingTeam!.Players)
            {
                var matchPlayerScore = player.Scores.Single(s => s.Match.Id == match.Id);
                player.Mmr = _mmrCalculator.RecalculatePlayerMmr(player, winningTeamMmr, matchPlayerScore, 0);
                await _playerRepository.UpdatePlayerAsync(player);
            }
        }

        match.Status = matchStatus;

        await _matchRepository.UpdateMatchAsync(match);

        await _tournamentService.UpdateTournamentBracket(match.Round.Id);

        return match;
    }

    public async Task<Match> UpdateMatchWinnerAsync(int matchId, int winnerTeamId)
    {
        var match = await _matchRepository.GetMatchByIdAsync(matchId)
                    ?? throw new NotFoundException("Match with this id was not found.");

        var team = await _teamRepository.GetTeamByIdAsync(winnerTeamId)
                   ?? throw new NotFoundException("Team with this id was not found.");

        if (!match.Competitors.Any(c => c.Id == team.Id))
        {
            throw new BadRequestException("This team did not play in this match.");
        }

        if (match.Winner != null)
        {
            throw new BadRequestException("This match already has a winner");
        }

        if (match.Status != Status.InProgress)
        {
            throw new BadRequestException("Can only update match winner while the match is in progress");
        }

        match.Winner = new MatchWinner
        {
            Match = match,
            WinnerTeam = team,
        };

        await _matchRepository.UpdateMatchAsync(match);

        return match;
    }
}