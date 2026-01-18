using tMoney.Domain.ValueObjects;

namespace tMoney.Domain.BoundedContexts.CardContext.Entities;

public abstract class Card
{
    public IdValueObject Id { get; private set; }
    public IdValueObject AccountId { get; private set; }
    public string Name { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Card() { }

    public Card(IdValueObject accountId, string name)
    {
        Id = IdValueObject.New();
        AccountId = accountId;
        Name = name;
        UpdatedAt = null;
        CreatedAt = DateTime.UtcNow;

        ValidateDomain();
    }
    
    private void ValidateDomain()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new ArgumentException("O nome do cartão não pode ser nulo ou vazio.");
        if (Name.Length > 50)
            throw new ArgumentException("O nome do cartão não pode ultrapassar 50 caracteres.");
    }
}
