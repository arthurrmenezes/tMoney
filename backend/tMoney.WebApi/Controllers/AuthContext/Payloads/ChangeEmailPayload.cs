namespace tMoney.WebApi.Controllers.AuthContext.Payloads;

public sealed class ChangeEmailPayload
{
    public string NewEmail { get; init; }

    public ChangeEmailPayload(string newEmail)
    {
        NewEmail = newEmail;
    }
}
