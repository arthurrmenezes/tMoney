namespace tMoney.Domain.ValueObjects;

public sealed class IdValueObject
{
    public Guid Id { get; private set; }

    private IdValueObject(Guid id)
    {
        Id = id;
    }

    public static IdValueObject Factory(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("ID não pode ser nulo ou vazio.", nameof(id));

        return new IdValueObject(id);
    }

    public static IdValueObject New()
    {
        return new IdValueObject(Guid.NewGuid());
    }

    public override string ToString() => Id.ToString();

    public static implicit operator Guid(IdValueObject idValueObject) => idValueObject.Id;
    public static implicit operator IdValueObject(Guid id) => Factory(id);
}
