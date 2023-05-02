using ESTMS.API.DataAccess.Entities;
using ESTMS.API.Host.Models.Users;

namespace ESTMS.API.Host.Models.Tournament;

public class TournamentManagerResponse
{
    public int Id { get; set; }
    public UserResponse User { get; set; }
}