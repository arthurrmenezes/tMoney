namespace tMoney.Application.Services.AuthContext.Inputs;

public sealed class RefreshTokenServiceInput
{
    public string AccessToken { get; }
    public string RefreshToken { get; }

    private RefreshTokenServiceInput(string accessToken, string refreshToken)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }

    public static RefreshTokenServiceInput Factory(string accessToken, string refreshToken)
        => new(accessToken, refreshToken);
}
