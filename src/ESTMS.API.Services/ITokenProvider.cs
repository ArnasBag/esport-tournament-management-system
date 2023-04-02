namespace ESTMS.API.Services;

public interface ITokenProvider
{
    public string GetToken(string userId);
}
