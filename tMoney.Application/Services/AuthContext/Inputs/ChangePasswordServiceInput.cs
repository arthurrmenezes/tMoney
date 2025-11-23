namespace tMoney.Application.Services.AuthContext.Inputs;

public sealed class ChangePasswordServiceInput
{
    public string UserId { get; }
    public string CurrentPassword { get; }
    public string NewPassword { get; }

    private ChangePasswordServiceInput(string userId, string currentPassword, string newPassword)
    {
        UserId = userId;
        CurrentPassword = currentPassword;
        NewPassword = newPassword;
    }

    public static ChangePasswordServiceInput Factory(string userId, string currentPassword, string newPassword)
        => new(userId, currentPassword, newPassword);
}
