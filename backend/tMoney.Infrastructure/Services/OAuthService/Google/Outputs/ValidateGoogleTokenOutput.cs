namespace tMoney.Infrastructure.Services.OAuthService.Google.Outputs;

public sealed class ValidateGoogleTokenOutput
{
    public string FirstName { get; }
    public string LastName { get; }
    public string Email { get; }

    private ValidateGoogleTokenOutput(string firstName, string lastName, string email)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    public static ValidateGoogleTokenOutput Factory(string firstName, string lastName, string email)
        => new(firstName, lastName, email);
}
