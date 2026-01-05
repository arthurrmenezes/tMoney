namespace tMoney.Application.UseCases.AuthContext.GoogleAuthUseCase.Outputs;

public sealed class GoogleAuthUseCaseOutput
{
    public string AccountId { get; }
    public string? FirstName { get; }
    public string? LastName { get; }
    public string Email { get; }
    public string AccessToken { get; }
    public string TokenType { get; }
    public int ExpiresIn { get; }
    public string RefreshToken { get; }
    public DateTime? CreatedAt { get; }

    private GoogleAuthUseCaseOutput(string accountId, string? firstName, string? lastName, string email, string accessToken, string tokenType, 
        int expiresIn, string refreshToken, DateTime? createdAt)
    {
        AccountId = accountId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        AccessToken = accessToken;
        TokenType = tokenType;
        ExpiresIn = expiresIn;
        RefreshToken = refreshToken;
        CreatedAt = createdAt;
    }

    public static GoogleAuthUseCaseOutput Factory(string accountId, string? firstName, string? lastName, string email, string accessToken, 
        string tokenType, int expiresIn, string refreshToken, DateTime? createdAt)
        => new(accountId, firstName, lastName, email, accessToken, tokenType, expiresIn, refreshToken, createdAt);
}
