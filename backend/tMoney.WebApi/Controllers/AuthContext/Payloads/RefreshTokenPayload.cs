namespace tMoney.WebApi.Controllers.AuthContext.Payloads;

public sealed class RefreshTokenPayload
{
    public string AccessToken { get; init; }
    public string RefreshToken { get; init; }

    public RefreshTokenPayload(string accessToken, string refreshToken)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
}
