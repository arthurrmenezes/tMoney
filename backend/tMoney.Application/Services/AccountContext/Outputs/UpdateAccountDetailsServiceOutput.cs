using tMoney.Domain.ValueObjects;

namespace tMoney.Application.Services.AccountContext.Outputs;

public sealed class UpdateAccountDetailsServiceOutput
{
    public string AccountId { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string Email { get; }
    public decimal Balance { get; }
    public DateTime? LastLoginAt { get; }
    public DateTime? UpdatedAt { get; }
    public DateTime CreatedAt { get; }

    private UpdateAccountDetailsServiceOutput(string accountId, string firstName, string lastName, string email, decimal balance, DateTime? lastLoginAt,
        DateTime? updatedAt, DateTime createdAt)
    {
        AccountId = accountId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Balance = balance;
        LastLoginAt = lastLoginAt;
        UpdatedAt = updatedAt;
        CreatedAt = createdAt;
    }

    public static UpdateAccountDetailsServiceOutput Factory(string accountId, FirstNameValueObject firstName, LastNameValueObject lastName,
        EmailValueObject email, decimal balance, DateTime? lastLoginAt, DateTime? updatedAt, DateTime createdAt)
        => new(accountId, firstName.FirstName, lastName.LastName, email.Email, balance, lastLoginAt, updatedAt, createdAt);
}
