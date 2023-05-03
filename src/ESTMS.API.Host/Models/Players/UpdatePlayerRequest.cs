namespace ESTMS.API.Host.Models.Player;

public class UpdatePlayerRequest
{
    public PlayersUserInfo? UserInfo { get; set; }
    public string? AboutMeText { get; set; }
    public IFormFile? ProfilePicture { get; set; }
}