using tMoney.Application.Services.AuthContext.Inputs;
using tMoney.Application.Services.AuthContext.Outputs;

namespace tMoney.Application.Services.AuthContext.Interfaces;

public interface IAuthService
{
    public Task<RegisterAccountServiceOutput> RegisterAccountServiceAsync(
        RegisterAccountServiceInput input,
        CancellationToken cancellationToken);

    public Task<LoginServiceOutput> LoginServiceAsync(
        LoginServiceInput input,
        CancellationToken cancellationToken);

    public Task<RefreshTokenServiceOutput> RefreshTokenServiceAsync(
        RefreshTokenServiceInput input,
        CancellationToken cancellationToken);

    public Task LogoutServiceAsync(
        string userId,
        CancellationToken cancellationToken);

    public Task ConfirmEmailServiceAsync(
        string email,
        string emailToken);

    public Task ResendConfirmationEmailServiceAsync(
        string email,
        CancellationToken cancellationToken);

    public Task ChangePasswordServiceAsync(
        ChangePasswordServiceInput input,
        CancellationToken cancellationToken);

    public Task ForgotPasswordServiceAsync(
        string email,
        CancellationToken cancellationToken);

    public Task ResetPasswordServiceAsync(
        ResetPasswordServiceInput input,
        CancellationToken cancellationToken);
}
