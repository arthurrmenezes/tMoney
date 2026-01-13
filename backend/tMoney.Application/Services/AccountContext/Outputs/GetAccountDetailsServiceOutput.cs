namespace tMoney.Application.Services.AccountContext.Outputs;

public sealed class GetAccountDetailsServiceOutput
{
    public string AccountId { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string Email { get; }
    public DateTime? LastLoginAt { get; }
    public DateTime? UpdatedAt { get; }
    public DateTime CreatedAt { get; }

    private GetAccountDetailsServiceOutput(string accountId, string firstName, string lastName, string email, DateTime? lastLoginAt, DateTime? updatedAt, 
        DateTime createdAt)
    {
        AccountId = accountId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        LastLoginAt = lastLoginAt;
        UpdatedAt = updatedAt;
        CreatedAt = createdAt;
    }

    public static GetAccountDetailsServiceOutput Factory(string accountId, string firstName, string lastName, string email, DateTime? lastLoginAt, 
        DateTime? updatedAt, DateTime createdAt)
        => new(accountId, firstName, lastName, email, lastLoginAt, updatedAt, createdAt);
}
