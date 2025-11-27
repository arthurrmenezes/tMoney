namespace tMoney.Application.Services.AccountContext.Inputs;

public sealed class UpdateAccountDetailsServiceInput
{
    public string? FirstName { get; }
    public string? LastName { get; }

    private UpdateAccountDetailsServiceInput(string? firstName, string? lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public static UpdateAccountDetailsServiceInput Factory(string? firstName, string? lastName)
        => new(firstName, lastName);
}
