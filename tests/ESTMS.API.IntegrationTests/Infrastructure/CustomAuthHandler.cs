using ESTMS.API.DataAccess.Constants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace ESTMS.API.IntegrationTests.Infrastructure;

public class CustomAuthHandlerOptions : AuthenticationSchemeOptions
{
    public string DefaultUserId { get; set; } = null!;
}

public class CustomAuthHandler : AuthenticationHandler<CustomAuthHandlerOptions>
{
    public const string UserId = "UserId";
    public const string AuthenticationScheme = "Test";
    private readonly string _defaultUserId;

    public CustomAuthHandler(IOptionsMonitor<CustomAuthHandlerOptions> options,
        ILoggerFactory logger, 
        UrlEncoder encoder, 
        ISystemClock clock) : base(options, logger, encoder, clock)
    {
        _defaultUserId = options.CurrentValue.DefaultUserId;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new List<Claim> { new Claim(ClaimTypes.Name, "Test user") };

        // Extract User ID from the request headers if it exists,
        // otherwise use the default User ID from the options.
        if (Context.Request.Headers.TryGetValue(UserId, out var userId))
        {
            claims.Add(new Claim(ClaimTypes.NameIdentifier, userId[0]));
        }
        else
        {
            claims.Add(new Claim(ClaimTypes.NameIdentifier, _defaultUserId));
        }

        // TODO: Add as many claims as you need here
        claims.Add(new Claim(ClaimTypes.Role, Roles.Player));
        claims.Add(new Claim(ClaimTypes.Role, Roles.Admin));


        var identity = new ClaimsIdentity(claims, AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, AuthenticationScheme);

        var result = AuthenticateResult.Success(ticket);

        return Task.FromResult(result);
    }
}
