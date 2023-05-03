using ESTMS.API.DataAccess.Entities;

namespace ESTMS.API.Services.Matches;

public class EloRatingCalculator : IMmrCalculator
{
    public int RecalculatePlayerMmr(Player player, int opponentTeamMmr, PlayerScore playerScore, int matchOutcome)
    {
        var expectedOutcome = 1 / (1 + Math.Pow(10, (opponentTeamMmr - player.Mmr) / 400));

        var kFactor = 20;

        var newMmr = (int)(player.Mmr + kFactor * (matchOutcome - expectedOutcome));

        return newMmr;
    }
}
