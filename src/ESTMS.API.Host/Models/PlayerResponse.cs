namespace ESTMS.API.Host.Models;

public class PlayerResponse
{
    public int Id { get; set; }
    public List<InvitationResponse> Invitations { get; set; } = new();
    public TeamResponse? Team { get; set; }
    public UserResponse ApplicationUser { get; set; } = new();
    public string PicturePath { get; set; } = string.Empty;
    public string AboutMeText { get; set; } = string.Empty;
    public int Points { get; set; }
    public int Rank { get; set; } = new();
}