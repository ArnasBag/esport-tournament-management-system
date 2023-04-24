namespace ESTMS.API.DataAccess.Entities;

public class TournamentManager
{
    public int Id { get; set; }
    public List<Tournament> Tournaments { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
}