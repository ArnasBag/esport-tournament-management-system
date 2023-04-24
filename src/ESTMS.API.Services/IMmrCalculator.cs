using ESTMS.API.DataAccess.Entities;

namespace ESTMS.API.Services;

public interface IMmrCalculator
{
    int RecalculatePlayerMmr(Player player, int opponentTeamMmr, PlayerScore playerScore, int matchOutcome);
}
