using ESTMS.API.Core.Exceptions;
using ESTMS.API.DataAccess.Constants;
using ESTMS.API.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;

namespace ESTMS.API.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenProvider _tokenProvider;

    public AuthService(UserManager<ApplicationUser> userManager, ITokenProvider tokenProvider)
    {
        _userManager = userManager;
        _tokenProvider = tokenProvider;
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

        await _userManager.AddToRoleAsync(user!, Roles.Member);
    }
}
