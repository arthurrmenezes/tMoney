namespace tMoney.WebApi.Controllers.AuthContext.Payloads;

public sealed class ResendConfirmationEmailPayload
{
    public string Email { get; init; }

    public ResendConfirmationEmailPayload(string email)
    {
        Email = email;
    }
}
