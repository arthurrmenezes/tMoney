namespace tMoney.Application.Services.AuthContext.Inputs;

public sealed class RefreshTokenServiceInput
{
    public string RefreshToken { get; }

    private RefreshTokenServiceInput(string refreshToken)
    {
        RefreshToken = refreshToken;
    }

    public static RefreshTokenServiceInput Factory(string refreshToken)
        => new(refreshToken);
}
