using tMoney.Domain.ValueObjects;

namespace tMoney.Domain.BoundedContexts.AccountContext.Entities;

public class Account
{
    public IdValueObject AccountId { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public decimal Balance { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Account(string firstName, string lastName, string email, decimal balance)
    {
        AccountId = IdValueObject.New();
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Balance = 0;
        CreatedAt = DateTime.UtcNow;
    }
}
