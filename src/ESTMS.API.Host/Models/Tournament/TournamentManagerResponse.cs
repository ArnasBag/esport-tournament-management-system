using ESTMS.API.DataAccess.Entities;

namespace ESTMS.API.Host.Models.Tournament;

public class TournamentManagerResponse
{
    public int Id { get; set; }
    public UserResponse User { get; set; }
}