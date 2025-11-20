using tMoney.Application.Services.AuthContext.Inputs;
using tMoney.Application.Services.AuthContext.Outputs;

namespace tMoney.Application.Services.AuthContext.Interfaces;

public interface IAuthService
{
    public Task<RegisterAccountServiceOutput> RegisterAccountServiceAsync(
        RegisterAccountServiceInput input,
        CancellationToken cancellationToken);
}
