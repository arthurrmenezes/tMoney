using tMoney.Domain.ValueObjects.Base;

namespace tMoney.Domain.ValueObjects;

public sealed class IdValueObject : ValueObject<IdValueObject>
{
    public Guid Id { get; private set; }

    private IdValueObject(Guid id)
    {
        Id = id;
    }

    public static IdValueObject Factory(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("ID não pode ser nulo ou vazio.");

        return new IdValueObject(id);
    }

    public static IdValueObject New()
    {
        return new IdValueObject(Guid.NewGuid());
    }

    public override string ToString() => Id.ToString();

    protected override bool EqualsCore(IdValueObject other)
    {
        return Id == other.Id;
    }

    protected override decimal GetHashCodeCore()
    {
        decimal hashCode = Id.GetHashCode();
        return hashCode;
    }
}
