using ESTMS.API.Core.Exceptions;
using ESTMS.API.DataAccess.Entities;
using ESTMS.API.DataAccess.Repositories;

namespace ESTMS.API.Services;

public class PlayerService : IPlayerService
{
    private readonly IPlayerRepository _playerRepository;

    public PlayerService(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    public async Task<Player> GetPlayerByIdAsync(int id)
    {
        return await _playerRepository.GetPlayerByIdAsync(id) ??
               throw new NotFoundException("Player with this id doesn't exist.");
    }

    public async Task<List<Player>> GetAllPlayersAsync(string? userId = null)
    {
        return await _playerRepository.GetAllPlayersAsync();
    }

    public async Task<Player> UpdatePlayerAsync(int id, Player updatedPlayer)
    {
        var player = await _playerRepository.GetPlayerByIdAsync(id)
                     ?? throw new NotFoundException("Player with this id doesn't exist.");

        player.ApplicationUser.UserName = updatedPlayer.ApplicationUser.UserName;
        player.ApplicationUser.NormalizedUserName = updatedPlayer.ApplicationUser.NormalizedUserName;
        player.AboutMeText = updatedPlayer.AboutMeText;
        player.PicturePath = updatedPlayer.PicturePath;

        return await _playerRepository.UpdatePlayerAsync(player);
    }

    public async Task<Player> UpdatePlayersRankAsync(int id)
    {
        var player = await _playerRepository.GetPlayerByIdAsync(id)
                     ?? throw new NotFoundException("Player with this id doesn't exist.");

        var playerPoints = player.Points;
        Rank playerRank;

        switch (playerPoints)
        {
            case < 50:
                playerRank = Rank.Bronze;
                break;
            case < 100:
                playerRank = Rank.Silver;
                break;
            case < 150:
                playerRank = Rank.Gold;
                break;
            case < 200:
                playerRank = Rank.Platinum;
                break;
            default:
                playerRank = Rank.Bronze;
                break;
        }

        player.Rank = playerRank;

        return await _playerRepository.UpdatePlayerAsync(player);
    }

    public async Task<Player> UpdatePlayersPointAsync(int id, int points)
    {
        var player = await _playerRepository.GetPlayerByIdAsync(id) ??
                     throw new NotFoundException("Player with this id doesn't exist.");

        if (player.Points is null)
        {
            player.Points = 0;
        }

        player.Points += points;

        if (player.Points < 0)
        {
            player.Points = 0;
        }

        await _playerRepository.UpdatePlayerAsync(player);
        return await UpdatePlayersRankAsync(player.Id);
    }
}