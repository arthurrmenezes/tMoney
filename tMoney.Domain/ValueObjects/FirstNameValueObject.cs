using System.Globalization;
using tMoney.Domain.ValueObjects.Base;

namespace tMoney.Domain.ValueObjects;

public sealed class FirstNameValueObject : ValueObject<FirstNameValueObject>
{
    public string FirstName { get; private set; }

    public const int MinLength = 3;
    public const int MaxLength = 50;

    private FirstNameValueObject(string firstName)
    {
        FirstName = firstName;
    }

    public static FirstNameValueObject Factory(string firstName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("O primeiro nome não pode ser nulo ou vazio.");

        firstName = firstName.Trim();

        if (firstName.Length < MinLength)
            throw new ArgumentException($"O primeiro nome deve ter pelo menos {MinLength} caracteres.");

        if (firstName.Length > MaxLength)
            throw new ArgumentException($"O primeiro nome não pode ultrapassar {MaxLength} caracteres.");

        if (firstName.Any(char.IsDigit))
            throw new ArgumentException("O primeiro nome não pode conter números.");

        string formattedFirstName = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(firstName.ToLowerInvariant());

        return new FirstNameValueObject(formattedFirstName);
    }

    public override string ToString() => FirstName;

    protected override bool EqualsCore(FirstNameValueObject other)
    {
        return FirstName == other.FirstName;
    }

    protected override decimal GetHashCodeCore()
    {
        decimal hashCode = FirstName.GetHashCode();
        return hashCode;
    }
}
