using ESTMS.API.DataAccess.Data;
using ESTMS.API.DataAccess.Entities;
using ESTMS.API.Host.Identity;
using ESTMS.API.Host.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace ESTMS.API.Host.Capabilities;

public static class StartupIdentity
{
    public static IServiceCollection ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var authSettings = configuration.GetSection("Auth0").Get<AuthSettings>() ??
            throw new ArgumentNullException(nameof(configuration));

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "estmsapi",
                    ValidAudience = "estmsapi",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("randomrandomrandomrandomrandomrandomrandomrandomrandomrandom"))
                };
            });

        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 3;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>();

        return services;
    }

    public static IServiceCollection ConfigureAuthorization(this IServiceCollection services, 
        params string[] scopes)
    {
        services.AddAuthorization(options =>
        {
            foreach (var scope in scopes)
            {
                options.AddPolicy(scope, policy => policy.RequireClaim("scope", scope));
            }
        });

        return services;
    }
}
