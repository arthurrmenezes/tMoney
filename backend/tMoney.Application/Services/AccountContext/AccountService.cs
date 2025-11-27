using tMoney.Application.Services.AccountContext.Interfaces;
using tMoney.Application.Services.AccountContext.Outputs;
using tMoney.Domain.ValueObjects;
using tMoney.Infrastructure.Data.Repositories.Interfaces;

namespace tMoney.Application.Services.AccountContext;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;

    public AccountService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<GetAccountDetailsServiceOutput> GetAccountDetailsServiceAsync(IdValueObject accountId, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetAccountByIdAsync(accountId.Id, cancellationToken);
        if (account is null)
            throw new KeyNotFoundException($"Conta com ID {accountId.ToString()} não foi encontrada.");

        var output = GetAccountDetailsServiceOutput.Factory(
            accountId: account.AccountId,
            firstName: account.FirstName,
            lastName: account.LastName,
            email: account.Email,
            balance: account.Balance,
            createdAt: account.CreatedAt);

        return output;
    }
}
