using Microsoft.AspNetCore.Identity;
using tMoney.Application.Services.AuthContext.Inputs;
using tMoney.Application.Services.AuthContext.Interfaces;
using tMoney.Application.Services.AuthContext.Outputs;
using tMoney.Domain.BoundedContexts.AccountContext.Entities;
using tMoney.Infrastructure.Auth.Entities;
using tMoney.Infrastructure.Data.Repositories.Interfaces;
using tMoney.Infrastructure.Data.UnitOfWork.Interfaces;

namespace tMoney.Application.Services.AuthContext;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAccountRepository _accountRepository;

    public AuthService(UserManager<User> userManager, IUnitOfWork unitOfWork, IAccountRepository accountRepository)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _accountRepository = accountRepository;
    }

    public async Task<RegisterAccountServiceOutput> RegisterAccountServiceAsync(RegisterAccountServiceInput input, CancellationToken cancellationToken)
    {
        var emailString = input.Email.ToString();
        var emailExists = await _userManager.FindByEmailAsync(emailString);
        if (emailExists is not null)
            throw new ArgumentException("O email já está em uso.");

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var account = new Account(
                firstName: input.FirstName,
                lastName: input.LastName,
                email: input.Email);

            await _accountRepository.AddAsync(account, cancellationToken);

            var user = new User
            {
                UserName = emailString,
                Email = emailString,
                AccountId = account.AccountId
            };

            var result = await _userManager.CreateAsync(user, input.Password);

            if (!result.Succeeded)
            {
                var error = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new ArgumentException($"Falha ao criar conta: {error}");
            }

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            var output = RegisterAccountServiceOutput.Factory(
                accountId: account.AccountId,
                firstName: account.FirstName,
                lastName: account.LastName,
                email: account.Email,
                balance: account.Balance,
                createdAt: account.CreatedAt);

            return output;
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}
