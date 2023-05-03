using ESTMS.API.DataAccess.Entities;
using ESTMS.API.Services.Matches;
using Moq;
using NUnit.Framework;

namespace ESTMS.API.UnitTests;

public class EloRatingCalculatorTests
{
    private IMmrCalculator _eloRatingCalculator;

    [SetUp]
    public void Setup()
    {
        _eloRatingCalculator = new EloRatingCalculator();
    }

    public static IEnumerable<TestCaseData> EloRatingCalculatorTestCases()
    {
        yield return new TestCaseData(1300, 1000, 1, 1310);
        yield return new TestCaseData(1300, 1000, 0, 1290);
        yield return new TestCaseData(1000, 1000, 1, 1010);
        yield return new TestCaseData(1000, 1000, 0, 990);
    }

    [Test]
    [TestCaseSource(nameof(EloRatingCalculatorTestCases))]
    public void RecalculatePlayerMmr_ValidData_CorrectlyCalculatesMmrRating(int playerMmr, int opponentMmr, int outcome, int expectedMmr)
    {
        var player = new Player 
        { 
            Mmr = playerMmr,
            Scores = new()
            {
                new PlayerScore()
            }
        };

        var newMmr = _eloRatingCalculator.RecalculatePlayerMmr(player, opponentMmr, It.IsAny<PlayerScore>(), outcome);

        Assert.That(newMmr, Is.EqualTo(expectedMmr));
    }
}
