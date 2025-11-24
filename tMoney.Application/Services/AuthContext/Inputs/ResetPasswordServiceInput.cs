namespace tMoney.Application.Services.AuthContext.Inputs;

public sealed class ResetPasswordServiceInput
{
    public string Email { get; }
    public string EmailToken { get; }
    public string NewPassword { get; }

    private ResetPasswordServiceInput(string email, string emailToken, string newPassword)
    {
        Email = email;
        EmailToken = emailToken;
        NewPassword = newPassword;
    }

    public static ResetPasswordServiceInput Factory(string email, string emailToken, string newPassword)
        => new(email, emailToken, newPassword);
}
