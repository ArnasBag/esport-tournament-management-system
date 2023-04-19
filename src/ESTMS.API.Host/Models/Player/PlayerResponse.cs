using ESTMS.API.DataAccess.Entities;

namespace ESTMS.API.Host.Models.Player;

public class PlayerResponse
{
    public int Id { get; set; }
    public UserResponse ApplicationUser { get; set; } = new();
    public string PicturePath { get; set; } = string.Empty;
    public string AboutMeText { get; set; } = string.Empty;
    public PlayersTeamResponse? Team { get; set; }
    public int Points { get; set; }
    public Rank Rank { get; set; }
}