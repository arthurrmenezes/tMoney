using tMoney.Domain.ValueObjects;

namespace tMoney.Domain.BoundedContexts.AccountContext.Entities;

public class Account
{
    public IdValueObject AccountId { get; private set; }
    public FirstNameValueObject FirstName { get; private set; }
    public LastNameValueObject LastName { get; private set; }
    public EmailValueObject Email { get; private set; }
    public decimal Balance { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Account(FirstNameValueObject firstName, LastNameValueObject lastName, EmailValueObject email)
    {
        AccountId = IdValueObject.New();
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Balance = 0;
        CreatedAt = DateTime.UtcNow;
    }
}
