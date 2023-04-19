namespace ESTMS.API.Host.Models.Player;

public class PlayersTeamResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public UserResponse TeamManager { get; set; } = new();
}