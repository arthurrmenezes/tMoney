using tMoney.Domain.ValueObjects;

namespace tMoney.Application.Services.AccountContext.Outputs;

public sealed class GetAccountDetailsServiceOutput
{
    public string AccountId { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string Email { get; }
    public decimal Balance { get; }
    public DateTime CreatedAt { get; }

    private GetAccountDetailsServiceOutput(string accountId, string firstName, string lastName, string email, decimal balance, DateTime createdAt)
    {
        AccountId = accountId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Balance = balance;
        CreatedAt = createdAt;
    }

    public static GetAccountDetailsServiceOutput Factory(string accountId, string firstName, string lastName, string email, decimal balance, 
        DateTime createdAt)
        => new(accountId, firstName, lastName, email, balance, createdAt);
}
