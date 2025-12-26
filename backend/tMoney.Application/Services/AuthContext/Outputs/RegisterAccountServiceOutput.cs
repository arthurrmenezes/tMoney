using tMoney.Domain.ValueObjects;

namespace tMoney.Application.Services.AuthContext.Outputs;

public sealed class RegisterAccountServiceOutput
{
    public IdValueObject AccountId { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string Email { get; }
    public decimal Balance { get; }
    public DateTime CreatedAt { get; }

    private RegisterAccountServiceOutput(IdValueObject accountId, string firstName, string lastName, string email, decimal balance, DateTime createdAt)
    {
        AccountId = accountId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Balance = balance;
        CreatedAt = createdAt;
    }

    public static RegisterAccountServiceOutput Factory(IdValueObject accountId, FirstNameValueObject firstName, LastNameValueObject lastName, 
        EmailValueObject email, decimal balance, DateTime createdAt)
        => new (accountId, firstName.FirstName, lastName.LastName, email.Email, balance, createdAt);
}
