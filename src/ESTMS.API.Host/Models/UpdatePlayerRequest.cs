using ESTMS.API.DataAccess.Entities;

namespace ESTMS.API.Host.Models;

public class UpdatePlayerRequest
{
    public List<Invitation> Invitations { get; set; }
    public Team? Team { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
    public string PicturePath { get; set; }
    public string AboutMeText { get; set; }
    public int Points { get; set; }
    public Rank Rank { get; set; }
}