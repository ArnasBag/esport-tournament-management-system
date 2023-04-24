using ESTMS.API.DataAccess.Entities;

namespace ESTMS.API.Host.Models;

public class MatchResponse
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Status Status { get; set; }
    public MatchWinner? Winner { get; set; }
}