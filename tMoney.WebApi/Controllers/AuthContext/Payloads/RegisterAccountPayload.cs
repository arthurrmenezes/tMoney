namespace tMoney.WebApi.Controllers.AuthContext.Payloads;

public sealed class RegisterAccountPayload
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
    public string RePassword { get; init; }

    public RegisterAccountPayload(string firstName, string lastName, string email, string password, string rePassword)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        RePassword = rePassword;
    }
}
