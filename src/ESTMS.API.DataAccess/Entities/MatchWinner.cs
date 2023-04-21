namespace ESTMS.API.DataAccess.Entities;

public class MatchWinner
{
    public int Id { get; set; }
    public Match Match { get; set; }
    public Team WinnerTeam { get; set; }
}