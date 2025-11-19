using System.Globalization;
using tMoney.Domain.ValueObjects.Base;

namespace tMoney.Domain.ValueObjects;

public sealed class LastNameValueObject : ValueObject<LastNameValueObject>
{
    public string LastName { get; private set; }

    public const int MinLength = 3;
    public const int MaxLength = 50;

    private LastNameValueObject(string lastName)
    {
        LastName = lastName;
    }

    public static LastNameValueObject Factory(string lastName)
    {
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("O primeiro nome não pode ser nulo ou vazio.");

        lastName = lastName.Trim();

        if (lastName.Length < MinLength)
            throw new ArgumentException($"O primeiro nome deve ter pelo menos {MinLength} caracteres.");

        if (lastName.Length > MaxLength)
            throw new ArgumentException($"O primeiro nome não pode ultrapassar {MaxLength} caracteres.");

        if (lastName.Any(char.IsDigit))
            throw new ArgumentException("O primeiro nome não pode conter números.");

        string formattedFirstName = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(lastName.ToLowerInvariant());

        return new LastNameValueObject(formattedFirstName);
    }

    public override string ToString() => LastName;

    protected override bool EqualsCore(LastNameValueObject other)
    {
        return LastName == other.LastName;
    }

    protected override decimal GetHashCodeCore()
    {
        decimal hashCode = LastName.GetHashCode();
        return hashCode;
    }
}
