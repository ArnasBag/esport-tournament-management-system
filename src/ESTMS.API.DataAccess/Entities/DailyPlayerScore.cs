namespace ESTMS.API.DataAccess.Entities;

public class DailyPlayerScore
{
    public int TotalKills { get; set; }
    public int TotalAssists { get; set; }
    public int TotalDeaths { get; set; }
    public DateTime Date { get; set; }
}
