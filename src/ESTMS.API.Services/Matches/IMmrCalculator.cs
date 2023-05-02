using ESTMS.API.DataAccess.Entities;

namespace ESTMS.API.Services.Matches;

public interface IMmrCalculator
{
    int RecalculatePlayerMmr(Player player, int opponentTeamMmr, PlayerScore playerScore, int matchOutcome);
}
