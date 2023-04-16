using ESTMS.API.Core.Exceptions;
using ESTMS.API.DataAccess.Entities;
using ESTMS.API.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using System.Transactions;

namespace ESTMS.API.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(IUserRepository userRepository, UserManager<ApplicationUser> userManager)
    {
        _userRepository = userRepository;
        _userManager = userManager;
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

        //remove from player table or add if role is changed to player
        //add to teammanager table if role is changed to teammanager

        try
        {
            var addRoleResult = await _userManager.AddToRoleAsync(user, role);

            if (!addRoleResult.Succeeded)
            {
                throw new BadRequestException(string.Join(",", addRoleResult.Errors.Select(e => e.Description)));
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
