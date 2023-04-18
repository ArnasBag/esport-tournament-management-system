namespace ESTMS.API.DataAccess.Entities;

public class Player
{
    public int Id { get; set; }
    public List<Invitation>? Invitations { get; set; }
    public Team? Team { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
    public string? PicturePath { get; set; }
    public string? AboutMeText { get; set; }
    public int? Points { get; set; }
    public Rank? Rank { get; set; }
}

public enum Rank
{
    Bronze = 1,
    Silver = 2,
    Gold = 3,
    Platinum = 4
}