namespace ESTMS.API.DataAccess.Entities;

public class TeamManager
{
    public int Id { get; set; }
    public List<Team> Teams { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
}
