using tMoney.Domain.ValueObjects;

namespace tMoney.Application.Services.AuthContext.Inputs;

public sealed class RegisterAccountServiceInput
{
    public FirstNameValueObject FirstName { get; }
    public LastNameValueObject LastName { get; }
    public EmailValueObject Email { get; }
    public string Password { get; }
    public string RePassword { get; }

    private RegisterAccountServiceInput(FirstNameValueObject firstName, LastNameValueObject lastName, EmailValueObject email, string password, string rePassword)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        RePassword = rePassword;
    }

    public static RegisterAccountServiceInput Factory(FirstNameValueObject firstName, LastNameValueObject lastName, EmailValueObject email, string password, 
        string rePassword)
        => new(firstName, lastName, email, password, rePassword);
}
