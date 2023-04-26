namespace ESTMS.API.Host.Models;

public class TeamResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string LogoUrl { get; set; } = string.Empty;
    public UserResponse TeamManager { get; set; } = new();
    public List<UserResponse> Players { get; set; } = new();
    public List<MatchResponse> Matches { get; set; } = new();
}
