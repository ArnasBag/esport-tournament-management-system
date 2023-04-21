namespace ESTMS.API.DataAccess.Entities;

public class Match
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Status Status { get; set; }
    public Tournament Tournament { get; set; }
    public MatchWinner? Winner { get; set; }
    public List<Team> Competitors { get; set; }
    //public List<PlayerScore> PlayerScores { get; set; }
}