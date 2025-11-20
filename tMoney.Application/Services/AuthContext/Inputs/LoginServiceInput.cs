namespace tMoney.Application.Services.AuthContext.Inputs;

public sealed class LoginServiceInput
{
    public string Email { get; }
    public string Password { get; }

    private LoginServiceInput(string email, string password)
    {
        Email = email;
        Password = password;
    }

    public static LoginServiceInput Factory(string email, string password)
        => new(email, password);
}
