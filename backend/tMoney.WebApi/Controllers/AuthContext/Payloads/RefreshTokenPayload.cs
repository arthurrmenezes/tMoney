namespace tMoney.WebApi.Controllers.AuthContext.Payloads;

public sealed class RefreshTokenPayload
{
    public string RefreshToken { get; init; }

    public RefreshTokenPayload(string refreshToken)
    {
        RefreshToken = refreshToken;
    }
}
