using ESTMS.API.Host.Models.Auth;
using ESTMS.API.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESTMS.API.Host.Controllers;

[ApiController]
[Route("auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IUserIdProvider _userIdProvider;

    public AuthenticationController(IAuthService authService, 
        IUserIdProvider userIdProvider)
    {
        _authService = authService;
        _userIdProvider = userIdProvider;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegistrationRequest request)
    {
        await _authService.RegisterUserAsync(request.Username, request.Email, request.Password);

        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var accessToken = await _authService.LoginUserAsync(request.Email, request.Password);

        return Ok(accessToken);
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetMe()
    {
        var response = await _authService.GetMeAsync(_userIdProvider.UserId);

        return Ok(new { Id = response.Item1.Id, Username = response.Item1.UserName, Role = response.Item2});
    }
}
