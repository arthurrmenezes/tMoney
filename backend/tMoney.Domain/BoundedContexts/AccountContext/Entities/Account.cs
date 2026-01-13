using tMoney.Domain.ValueObjects;

namespace tMoney.Domain.BoundedContexts.AccountContext.Entities;

public class Account
{
    public IdValueObject AccountId { get; private set; }
    public FirstNameValueObject FirstName { get; private set; }
    public LastNameValueObject LastName { get; private set; }
    public EmailValueObject Email { get; private set; }
    public DateTime? LastLoginAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }

    protected Account() { }

    public Account(FirstNameValueObject firstName, LastNameValueObject lastName, EmailValueObject email)
    {
        AccountId = IdValueObject.New();
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        UpdatedAt = null;
        LastLoginAt = null;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateAccountDetails(FirstNameValueObject? firstName, LastNameValueObject? lastName)
    {
        if (firstName is not null)
            FirstName = firstName;

        if (lastName is not null)
            LastName = lastName;

        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateLastLoginDate()
    {
        LastLoginAt = DateTime.UtcNow;
    }
}
