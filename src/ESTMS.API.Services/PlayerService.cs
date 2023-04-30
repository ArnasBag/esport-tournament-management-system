using ESTMS.API.Core.Exceptions;
using ESTMS.API.DataAccess.Entities;
using ESTMS.API.DataAccess.Repositories;
using Microsoft.AspNetCore.Http;

namespace ESTMS.API.Services;

public class PlayerService : IPlayerService
{
    private readonly IPlayerRepository _playerRepository;
    private readonly IMatchRepository _matchRepository;
    private readonly IFileUploader _fileUploader;

    public PlayerService(IPlayerRepository playerRepository, 
        IFileUploader fileUploader, IMatchRepository matchRepository)
    {
        _playerRepository = playerRepository;
        _fileUploader = fileUploader;
        _matchRepository = matchRepository;
    }

    public async Task<Player> GetPlayerByIdAsync(string id)
    {
        return await _playerRepository.GetPlayerByIdAsync(id) ??
               throw new NotFoundException("Player with this id doesn't exist.");
    }

    public async Task<List<Player>> GetAllPlayersAsync(string? userId = null)
    {
        return await _playerRepository.GetAllPlayersAsync();
    }

    public async Task<Player> UpdatePlayerAsync(string id, Player updatedPlayer, IFormFile profilePicture)
    {
        var player = await _playerRepository.GetPlayerByIdAsync(id)
                     ?? throw new NotFoundException("Player with this id doesn't exist.");

        if (profilePicture != null)
        {
            await _fileUploader.DeleteFileAsync(player.PicturePath);
            var newProfilePictureUrl = await _fileUploader.UploadFileAsync(profilePicture);
            player.PicturePath = newProfilePictureUrl;
        }

        player.ApplicationUser.UserName = updatedPlayer.ApplicationUser.UserName ?? player.ApplicationUser.UserName;
        player.ApplicationUser.FirstName = updatedPlayer.ApplicationUser.FirstName ?? player.ApplicationUser.FirstName;
        player.ApplicationUser.LastName = updatedPlayer.ApplicationUser.LastName ?? player.ApplicationUser.LastName;
        player.ApplicationUser.NormalizedUserName = updatedPlayer.ApplicationUser.NormalizedUserName ?? player.ApplicationUser.NormalizedUserName;
        player.AboutMeText = updatedPlayer.AboutMeText ?? player.AboutMeText;

        return await _playerRepository.UpdatePlayerAsync(player);
    }

    public async Task<Player> UpdatePlayersRankAsync(string id)
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

    public async Task<Player> UpdatePlayersPointAsync(string id, int points)
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
        return await UpdatePlayersRankAsync(player.ApplicationUser.Id);
    }

    public async Task<List<Match>> GetPlayerWonMatches(string id)
    {
        var matches = await _matchRepository.GetPlayerWonMatchesAsync(id);

        return matches;
    }
}