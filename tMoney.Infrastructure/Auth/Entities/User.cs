using Microsoft.AspNetCore.Identity;
using tMoney.Domain.ValueObjects;

namespace tMoney.Infrastructure.Auth.Entities;

public class User : IdentityUser
{
    public IdValueObject AccountId { get; set; }

    public User(IdValueObject accountId)
    {
        AccountId = accountId;
    }

    public User() { }
}
