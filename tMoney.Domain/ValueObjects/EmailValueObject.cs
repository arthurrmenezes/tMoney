using System.Text.RegularExpressions;
using tMoney.Domain.ValueObjects.Base;

namespace tMoney.Domain.ValueObjects;

public sealed class EmailValueObject : ValueObject<EmailValueObject>
{
    public string Email { get; private set; }

    public const int MaxLength = 255;
    private static readonly Regex _emailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

    private EmailValueObject(string email)
    {
        Email = email;
    }

    public static EmailValueObject Factory(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("O email não pode ser nulo ou vazio.");

        if (email.Length > MaxLength)
            throw new ArgumentException($"O email não pode exceder {MaxLength} caracteres.");

        if (!_emailRegex.IsMatch(email))
            throw new ArgumentException("O formato do email é invalido.");

        return new EmailValueObject(email.ToLowerInvariant());
    }
    public override string ToString() => Email!;

    public static implicit operator string(EmailValueObject email) => email.Email!;
    public static implicit operator EmailValueObject(string email) => Factory(email);

    protected override bool EqualsCore(EmailValueObject other)
    {
        return Email == other.Email;
    }

    protected override decimal GetHashCodeCore()
    {
        decimal hashCode = Email.GetHashCode();
        return hashCode;
    }
}
