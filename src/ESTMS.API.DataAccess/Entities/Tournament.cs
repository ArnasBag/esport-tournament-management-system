namespace ESTMS.API.DataAccess.Entities;

public class Tournament
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public TournamentWinner? Winner { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public Status Status { get; set; }
    public List<Team>? Teams { get; set; }
    public List<Match>? Matches { get; set; }
    public TournamentManager Manager { get; set; }
}

public enum Status
{
    NotStarted = 0,
    InProgress = 1,
    Done = 2
}

//public class PlayerScore
//{
//    public int Id { get; set; }
//    public int Kills { get; set; }
//    public int Deaths { get; set; }
//    public int Assists { get; set; } 
//    public double KDA { get; set; }
//    public Match Match { get; set; }
//    public Player Player { get; set; }
//}