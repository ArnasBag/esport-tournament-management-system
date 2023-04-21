namespace ESTMS.API.DataAccess.Entities;

public class MatchWinner
{
    public int Id { get; set; }
    public int MatchId { get; set; }
    public Match Match { get; set; }
    public int WinnerTeamId { get; set; }
    public Team? WinnerTeam { get; set; }
}