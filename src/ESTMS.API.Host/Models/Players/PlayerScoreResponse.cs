using ESTMS.API.Host.Models.Matches;
using ESTMS.API.Host.Models.Users;

namespace ESTMS.API.Host.Models.Players
{
    public class PlayerScoreResponse
    {
        public int Kills { get; set; }
        public int Deaths { get; set; }
        public int Assists { get; set; }
        public MatchResponse Match { get; set; }
        public UserResponse Player { get; set; }
    }
}
