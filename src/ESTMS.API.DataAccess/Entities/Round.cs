namespace ESTMS.API.DataAccess.Entities;

public class Round
{
    public int Id { get; set; }
    public Tournament Tournament { get; set; }
    public List<Match> Matches { get; set; }
}