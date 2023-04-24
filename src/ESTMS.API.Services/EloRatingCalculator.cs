using ESTMS.API.DataAccess.Entities;

namespace ESTMS.API.Services;

public class EloRatingCalculator : IMmrCalculator
{
    public int RecalculatePlayerMmr(Player player, int opponentTeamMmr, PlayerScore playerScore, int matchOutcome)
    {
        var expectedOutcome = 1 / (1 + Math.Pow(10, ((opponentTeamMmr - player.Mmr) / 400)));

        var kFactor = 32;

        if (player.Scores.Count > 30)
        {
            kFactor = 10;
        }
        else
        {
            kFactor = 16;
        }

        var newMmr = (int)(player.Mmr + kFactor * (matchOutcome - expectedOutcome));

        return newMmr;
    }
}
