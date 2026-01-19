using tMoney.Domain.BoundedContexts.CardContext.ENUMs;
using tMoney.Domain.ValueObjects;

namespace tMoney.Domain.BoundedContexts.CardContext.Entities;

public class DebitCard : Card
{
    public DebitCard(IdValueObject accountId, string name) : base(accountId, name)
    {
        Type = CardType.DebitCard;
    }
}
