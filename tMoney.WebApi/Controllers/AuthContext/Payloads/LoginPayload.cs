namespace tMoney.WebApi.Controllers.AuthContext.Payloads;

public sealed class LoginPayload
{
    public string Email { get; init; }
    public string Password { get; init; }

    public LoginPayload(string email, string password)
    {
        Email = email;
        Password = password;
    }
}
