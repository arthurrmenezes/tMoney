namespace tMoney.WebApi.Controllers.AccountContext.Payloads;

public sealed class UpdateAccountDetailsPayload
{
    public string? FirstName { get; init; }
    public string? LastName { get; init; }

    public UpdateAccountDetailsPayload(string? firstName, string? lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
}
