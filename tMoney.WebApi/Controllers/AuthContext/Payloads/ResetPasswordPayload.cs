namespace tMoney.WebApi.Controllers.AuthContext.Payloads;

public sealed class ResetPasswordPayload
{
    public string NewPassword { get; init; }
    public string ConfirmNewPassword { get; init; }

    public ResetPasswordPayload(string newPassword, string confirmNewPassword)
    {
        NewPassword = newPassword;
        ConfirmNewPassword = confirmNewPassword;
    }
}
