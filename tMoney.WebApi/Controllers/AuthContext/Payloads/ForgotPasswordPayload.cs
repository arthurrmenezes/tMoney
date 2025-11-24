namespace tMoney.WebApi.Controllers.AuthContext.Payloads;

public sealed class ForgotPasswordPayload
{
    public string Email { get; init; }

    public ForgotPasswordPayload(string email)
    {
        Email = email;
    }
}
