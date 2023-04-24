namespace ESTMS.API.DataAccess.Entities;

public class TournamentWinner
{
    public int Id { get; set; }
    public int TournamentId { get; set; }
    public Tournament Tournament { get; set; }
    public Team? WinnerTeam { get; set; }
}