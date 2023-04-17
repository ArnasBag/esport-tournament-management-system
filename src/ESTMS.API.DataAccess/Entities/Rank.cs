namespace ESTMS.API.DataAccess.Entities;

public class Rank
{
    public short Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public enum RankEnum
{
    Bronze = 1,
    Silver = 2,
    Gold = 3,
    Platinum = 4
}