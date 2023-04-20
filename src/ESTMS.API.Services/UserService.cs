using ESTMS.API.Core.Exceptions;
using ESTMS.API.DataAccess.Constants;
using ESTMS.API.DataAccess.Entities;
using ESTMS.API.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using System.Transactions;

namespace ESTMS.API.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly ITeamManagerRepository _teamManagerRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(IUserRepository userRepository, UserManager<ApplicationUser> userManager,
        IPlayerRepository playerRepository, ITeamManagerRepository teamManagerRepository)
    {
        _userRepository = userRepository;
        _userManager = userManager;
        _playerRepository = playerRepository;
        _teamManagerRepository = teamManagerRepository;
    }

    public async Task ChangeUserActivityAsync(string id, bool status)
    {
        var user = await _userRepository.GetUserByIdAsync(id)
            ?? throw new BadRequestException("User with this id doesn't exist.");

        user.Active = status;

        await _userRepository.UpdateUserAsync(user);
    }

    public async Task ChangeUserRoleAsync(string id, string role)
    {
        var user = await _userRepository.GetUserByIdAsync(id)
            ?? throw new BadRequestException("User with this id doesn't exist.");

        var userRole = await _userManager.GetRolesAsync(user);

        using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        var removeRoleResult = await _userManager.RemoveFromRoleAsync(user, userRole.Single());

        if (!removeRoleResult.Succeeded)
        {
            throw new BadRequestException(string.Join(",", removeRoleResult.Errors.Select(e => e.Description)));
        }

        if(userRole.SingleOrDefault() == Roles.Player)
        {
            var player = await _playerRepository.GetPlayerByIdAsync(id);
            await _playerRepository.RemovePlayerAsync(player);
        }
        else if(userRole.SingleOrDefault() == Roles.TeamManager)
        {
            var teamManager = await _userRepository.GetTeamManagerByUserIdAsync(id);
            await _teamManagerRepository.RemoveAsync(teamManager);
        }

        try
        {
            var addRoleResult = await _userManager.AddToRoleAsync(user, role);

            if (!addRoleResult.Succeeded)
            {
                throw new BadRequestException(string.Join(",", addRoleResult.Errors.Select(e => e.Description)));
            }

            if(role == Roles.Player)
            {
                await _userRepository.CreatePlayerAsync(new Player { ApplicationUser = user });
            }
            else if(role == Roles.TeamManager)
            {
                await _teamManagerRepository.CreateAsync(new TeamManager { ApplicationUser = user });
            }
        }
        catch(InvalidOperationException ex)
        {
            throw new BadRequestException(ex.Message);
        }

        transaction.Complete();
    }

    public async Task<ApplicationUser> GetUserByIdAsync(string id)
    {
        var user = await _userRepository.GetUserByIdAsync(id)
            ?? throw new BadRequestException("User with this id doesn't exist.");

        return user;
    }

    public async Task<List<ApplicationUser>> GetUsersAsync()
    {
        var users = await _userRepository.GetUsersAsync();

        return users;
    }
}
