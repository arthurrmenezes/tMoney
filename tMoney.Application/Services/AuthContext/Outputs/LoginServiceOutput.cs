namespace tMoney.Application.Services.AuthContext.Outputs;

public sealed class LoginServiceOutput
{
    public string AcessToken { get; }
    public string TokenType { get; }
    public int ExpiresInHours { get; }
    public string RefreshToken { get; }

    private LoginServiceOutput(string acessToken, string tokenType, int expiresInHours, string refreshToken)
    {
        AcessToken = acessToken;
        TokenType = tokenType;
        ExpiresInHours = expiresInHours;
        RefreshToken = refreshToken;
    }

    public static LoginServiceOutput Factory(string acessToken, string tokenType, int expiresInHours, string refreshToken)
        => new(acessToken, tokenType, expiresInHours, refreshToken);
}
