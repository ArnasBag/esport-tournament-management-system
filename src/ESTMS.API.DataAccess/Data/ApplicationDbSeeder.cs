using ESTMS.API.DataAccess.Constants;
using Microsoft.AspNetCore.Identity;

namespace ESTMS.API.DataAccess.Data;

public class ApplicationDbSeeder
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public ApplicationDbSeeder(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task SeedAsync()
    {
        var roles = new List<string>() { Roles.Player, Roles.Admin, Roles.TeamManager, Roles.TournamentManager };

        foreach (var role in roles)
        {
            var exists = await _roleManager.RoleExistsAsync(role);

            if (!exists)
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}
