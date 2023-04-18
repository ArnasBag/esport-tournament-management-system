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

    public Task<Player> CreatePlayerAsync(Player player)
    {
        throw new NotImplementedException();
    }

    public Task<Player> UpdatePlayerAsync(int id, Player updatedPlayer)
    {
        throw new NotImplementedException();
    }
}