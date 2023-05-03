using ESTMS.API.Host.Models.Users;

namespace ESTMS.API.Host.Models.Matches;

public class MatchTeamResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string LogoUrl { get; set; } = string.Empty;
    public List<UserResponse> Players { get; set; }
}
