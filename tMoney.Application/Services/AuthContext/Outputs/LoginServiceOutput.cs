namespace tMoney.Application.Services.AuthContext.Outputs;

public sealed class LoginServiceOutput
{
    public string AccessToken { get; }
    public string TokenType { get; }
    public int ExpiresIn { get; }
    public string RefreshToken { get; }

    private LoginServiceOutput(string accessToken, string tokenType, int expiresIn, string refreshToken)
    {
        AccessToken = accessToken;
        TokenType = tokenType;
        ExpiresIn = expiresIn;
        RefreshToken = refreshToken;
    }

    public static LoginServiceOutput Factory(string accessToken, string tokenType, int expiresIn, string refreshToken)
        => new(accessToken, tokenType, expiresIn, refreshToken);
}
