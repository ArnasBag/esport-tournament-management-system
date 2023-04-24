using ESTMS.API.Core.Exceptions;
using ESTMS.API.DataAccess.Constants;
using ESTMS.API.DataAccess.Entities;
using ESTMS.API.DataAccess.Repositories;
using ESTMS.API.DataAccess.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace ESTMS.API.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenProvider _tokenProvider;
    private readonly IUserRepository _userRepository;
    private readonly IOptionsMonitor<MmrSettings> _mmrSettings;

    public AuthService(UserManager<ApplicationUser> userManager,
        ITokenProvider tokenProvider,
        IUserRepository userRepository,
        IOptionsMonitor<MmrSettings> mmrSettings)
    {
        _userManager = userManager;
        _tokenProvider = tokenProvider;
        _userRepository = userRepository;
        _mmrSettings = mmrSettings;
    }

    public async Task<(ApplicationUser, string?)> GetMeAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        var roles = await _userManager.GetRolesAsync(user);

        return (user, roles.FirstOrDefault());
    }

    public async Task<string> LoginUserAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email) 
            ?? throw new BadRequestException();

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);

        if (!isPasswordValid)
        {
            throw new BadRequestException();
        }

        var roles = (await _userManager.GetRolesAsync(user)).ToList();

        var accessToken = _tokenProvider.GetToken(user, roles);

        return accessToken;
    }

    public async Task RegisterUserAsync(string username, string email, string password)
    {
        var result = await _userManager.CreateAsync(
            new ApplicationUser { UserName = username, Email = email }, password);

        if (!result.Succeeded)
        {
            throw new BadRequestException(string.Join(",", result.Errors.Select(x => x.Description)));
        }

        var user = await _userManager.FindByEmailAsync(email);

        await _userManager.AddToRoleAsync(user!, Roles.Player);
        await _userRepository.CreatePlayerAsync(new Player 
        { 
            ApplicationUser = user!,
            Mmr = _mmrSettings.CurrentValue.IntialRating
        });
    }
}
