using tMoney.Application.Services.TransactionContext.Inputs;
using tMoney.Application.Services.TransactionContext.Interfaces;
using tMoney.Application.Services.TransactionContext.Outputs;
using tMoney.Domain.BoundedContexts.TransactionContext.Entities;
using tMoney.Domain.BoundedContexts.TransactionContext.ENUMs;
using tMoney.Infrastructure.Data.Repositories.Interfaces;
using tMoney.Infrastructure.Data.UnitOfWork.Interfaces;

namespace tMoney.Application.Services.TransactionContext;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAccountRepository _accountRepository;
    private readonly ICategoryRepository _categoryRepository;

    public TransactionService(ITransactionRepository transactionRepository, IUnitOfWork unitOfWork, IAccountRepository accountRepository, 
        ICategoryRepository categoryRepository)
    {
        _transactionRepository = transactionRepository; 
        _unitOfWork = unitOfWork;
        _accountRepository = accountRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<CreateTransactionServiceOutput> CreateTransactionServiceAsync(
        CreateTransactionServiceInput input, 
        CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetAccountByIdAsync(input.AccountId.Id, cancellationToken);
        if (account is null)
            throw new KeyNotFoundException("Conta não encontrada");

        var category = await _categoryRepository.GetByIdAsync(input.CategoryId.Id, input.AccountId.Id, cancellationToken);
        if (category is null)
            throw new KeyNotFoundException("Categoria não encontrada");

        var validateDestination = input.TransactionType == TransactionType.Income ? null : input.Destination;

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var transaction = new Transaction(
            accountId: input.AccountId,
            categoryId: input.CategoryId,
            title: input.Title,
            description: input.Description,
            amount: input.Amount,
            date: input.Date,
            transactionType: input.TransactionType,
            paymentMethod: input.PaymentMethod,
            status: input.Status,
            destination: validateDestination);

            await _transactionRepository.AddAsync(transaction, cancellationToken);

            if (input.Status == PaymentStatus.Paid)
            {
                if (input.TransactionType == TransactionType.Income)
                    account.IncrementBalance(input.Amount);
                else
                    account.DecrementBalance(input.Amount);

                _accountRepository.Update(account);
            }

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            var output = CreateTransactionServiceOutput.Factory(
                id: transaction.Id.ToString(),
                accountId: transaction.AccountId.ToString(),
                categoryId: transaction.CategoryId.ToString(),
                title: transaction.Title,
                description: transaction.Description,
                amount: transaction.Amount,
                date: transaction.Date,
                transactionType: transaction.TransactionType.ToString(),
                paymentMethod: transaction.PaymentMethod.ToString(),
                status: transaction.Status.ToString(),
                destination: transaction.Destination,
                updatedAt: transaction.UpdatedAt,
                createdAt: transaction.CreatedAt);

            return output;
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}
