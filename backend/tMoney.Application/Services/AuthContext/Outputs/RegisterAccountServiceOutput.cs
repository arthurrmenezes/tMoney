using tMoney.Domain.ValueObjects;

namespace tMoney.Application.Services.AuthContext.Outputs;

public sealed class RegisterAccountServiceOutput
{
    public IdValueObject AccountId { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string Email { get; }
    public DateTime CreatedAt { get; }

    private RegisterAccountServiceOutput(IdValueObject accountId, string firstName, string lastName, string email, DateTime createdAt)
    {
        AccountId = accountId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        CreatedAt = createdAt;
    }

    public static RegisterAccountServiceOutput Factory(IdValueObject accountId, FirstNameValueObject firstName, LastNameValueObject lastName, 
        EmailValueObject email, DateTime createdAt)
        => new (accountId, firstName.FirstName, lastName.LastName, email.Email, createdAt);
}
