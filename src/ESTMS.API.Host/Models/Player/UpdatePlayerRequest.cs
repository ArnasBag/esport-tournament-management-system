namespace ESTMS.API.Host.Models.Player;

public class UpdatePlayerRequest
{
    public PlayersUserInfo UserInfo { get; set; }
    public string PicturePath { get; set; }
    public string AboutMeText { get; set; }
    public int TeamId { get; set; }
}