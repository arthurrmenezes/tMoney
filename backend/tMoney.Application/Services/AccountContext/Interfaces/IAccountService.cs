using tMoney.Application.Services.AccountContext.Inputs;
using tMoney.Application.Services.AccountContext.Outputs;
using tMoney.Domain.ValueObjects;

namespace tMoney.Application.Services.AccountContext.Interfaces;

public interface IAccountService
{
    public Task<GetAccountDetailsServiceOutput> GetAccountDetailsServiceAsync(
        IdValueObject accountId,
        CancellationToken cancellationToken);

    public Task<UpdateAccountDetailsServiceOutput> UpdateAccountDetailsServiceAsync(
        IdValueObject accountId,
        UpdateAccountDetailsServiceInput input,
        CancellationToken cancellationToken);
}
