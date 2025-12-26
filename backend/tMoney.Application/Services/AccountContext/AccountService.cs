using tMoney.Application.Services.AccountContext.Inputs;
using tMoney.Application.Services.AccountContext.Interfaces;
using tMoney.Application.Services.AccountContext.Outputs;
using tMoney.Domain.ValueObjects;
using tMoney.Infrastructure.Data.Repositories.Interfaces;
using tMoney.Infrastructure.Data.UnitOfWork.Interfaces;

namespace tMoney.Application.Services.AccountContext;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AccountService(IAccountRepository accountRepository, IUnitOfWork unitOfWork)
    {
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<GetAccountDetailsServiceOutput> GetAccountDetailsServiceAsync(IdValueObject accountId, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetAccountByIdAsync(accountId.Id, cancellationToken);
        if (account is null)
            throw new KeyNotFoundException($"Conta não foi encontrada.");

        var output = GetAccountDetailsServiceOutput.Factory(
            accountId: account.AccountId.ToString(),
            firstName: account.FirstName,
            lastName: account.LastName,
            email: account.Email,
            balance: account.Balance,
            createdAt: account.CreatedAt);

        return output;
    }

    public async Task<UpdateAccountDetailsServiceOutput> UpdateAccountDetailsServiceAsync(IdValueObject accountId, UpdateAccountDetailsServiceInput input,
        CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetAccountByIdAsync(accountId.Id, cancellationToken);
        if (account is null)
            throw new KeyNotFoundException($"Conta não foi encontrada.");

        var newFirstName = input.FirstName != null ? FirstNameValueObject.Factory(input.FirstName) : null;
        var newLastName = input.LastName != null ? LastNameValueObject.Factory(input.LastName) : null;

        account.UpdateAccountDetails(
            firstName: newFirstName,
            lastName: newLastName);

        _accountRepository.Update(account);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var output = UpdateAccountDetailsServiceOutput.Factory(
            accountId: account.AccountId.ToString(),
            firstName: account.FirstName,
            lastName: account.LastName,
            email: account.Email,
            balance: account.Balance,
            updatedAt: account.UpdatedAt,
            createdAt: account.CreatedAt);

        return output;
    }
}
