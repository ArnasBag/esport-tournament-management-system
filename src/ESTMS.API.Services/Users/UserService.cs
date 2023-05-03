using ESTMS.API.Core.Exceptions;
using ESTMS.API.DataAccess.Constants;
using ESTMS.API.DataAccess.Entities;
using ESTMS.API.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using System.Transactions;

namespace ESTMS.API.Services.Users;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly ITeamManagerRepository _teamManagerRepository;
    private readonly ITournamentManagerRepository _tournamentManagerRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(IUserRepository userRepository, UserManager<ApplicationUser> userManager,
        IPlayerRepository playerRepository, ITeamManagerRepository teamManagerRepository,
        ITournamentManagerRepository tournamentManagerRepository)
    {
        _userRepository = userRepository;
        _userManager = userManager;
        _playerRepository = playerRepository;
        _teamManagerRepository = teamManagerRepository;
        _tournamentManagerRepository = tournamentManagerRepository;
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

        if (userRole.SingleOrDefault() == Roles.Player)
        {
            var player = await _playerRepository.GetPlayerByIdAsync(id);
            await _playerRepository.RemovePlayerAsync(player);
        }
        else if (userRole.SingleOrDefault() == Roles.TeamManager)
        {
            var teamManager = await _userRepository.GetTeamManagerByUserIdAsync(id);
            await _teamManagerRepository.RemoveAsync(teamManager);
        }
        else if (userRole.SingleOrDefault() == Roles.TournamentManager)
        {
            var tournamentManager = await _userRepository.GetTournamentManagerByUserIdAsync(id);
            await _tournamentManagerRepository.RemoveAsync(tournamentManager);
        }

        try
        {
            var addRoleResult = await _userManager.AddToRoleAsync(user, role);

            if (!addRoleResult.Succeeded)
            {
                throw new BadRequestException(string.Join(",", addRoleResult.Errors.Select(e => e.Description)));
            }

            if (role == Roles.Player)
            {
                await _userRepository.CreatePlayerAsync(new Player { ApplicationUser = user });
            }
            else if (role == Roles.TeamManager)
            {
                await _teamManagerRepository.CreateAsync(new TeamManager { ApplicationUser = user });
            }
            else if (role == Roles.TournamentManager)
            {
                await _tournamentManagerRepository.CreateAsync(new TournamentManager { ApplicationUser = user });
            }
        }
        catch (InvalidOperationException ex)
        {
            throw new BadRequestException(ex.Message);
        }

        transaction.Complete();
    }

    public async Task<List<DailyUserCreatedCount>> GetDailyCreatedUsersAsync(DateTime from, DateTime to)
    {
        var users = await _userRepository.GetUsersAsync();

        var filteredUsers = users.Where(u => u.CreatedAt >= from && u.CreatedAt <= to).ToList();

        var dailyCreatedUserCount = filteredUsers
            .GroupBy(u => u.CreatedAt.Date)
            .Select(u => new DailyUserCreatedCount
            {
                Date = u.Key,
                TotalCreatedUsers = u.Count()
            })
            .ToList();

        var allDates = Enumerable.Range(0, (to - from).Days + 1)
            .Select(i => from.AddDays(i).Date)
            .ToList();

        var result = allDates
            .GroupJoin(
                dailyCreatedUserCount,
                d => d,
                c => c.Date,
                (d, c) => new DailyUserCreatedCount
                {
                    Date = d,
                    TotalCreatedUsers = c.Sum(x => x.TotalCreatedUsers)
                })
            .ToList();

        return result;
    }

    public async Task<ApplicationUser> GetUserByIdAsync(string id)
    {
        var user = await _userRepository.GetUserByIdAsync(id)
                   ?? throw new BadRequestException("User with this id doesn't exist.");

        return user;
    }

    public async Task<List<ApplicationUserWithRole>> GetUsersAsync()
    {
        var users = await _userRepository.GetUsersAsync();
        var usersWithRole = new List<ApplicationUserWithRole>();

        foreach (var user in users)
        {
            var userRole = await _userManager.GetRolesAsync(user);

            usersWithRole.Add(new()
            {
                Role = userRole.Single(),
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Active = user.Active
            });
        }

        return usersWithRole;
    }
}