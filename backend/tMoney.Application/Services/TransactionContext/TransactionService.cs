using tMoney.Application.Services.TransactionContext.Inputs;
using tMoney.Application.Services.TransactionContext.Interfaces;
using tMoney.Application.Services.TransactionContext.Outputs;
using tMoney.Domain.BoundedContexts.TransactionContext.Entities;
using tMoney.Domain.BoundedContexts.TransactionContext.ENUMs;
using tMoney.Domain.ValueObjects;
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

    public async Task<GetTransactionByIdServiceOutput> GetTransactionByIdServiceAsync(
        IdValueObject transactionId, 
        IdValueObject accountId, 
        CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetByIdAsync(transactionId.Id, accountId.Id, cancellationToken);
        if (transaction is null)
            throw new KeyNotFoundException("Transação não encontrada");

        var output = GetTransactionByIdServiceOutput.Factory(
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

    public async Task<GetAllTransactionsByAccountIdServiceOutput> GetAllTransactionsByAccountIdServiceAsync(
        IdValueObject accountId, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var transactions = await _transactionRepository.GetAllByAccountIdAsync(accountId.Id, pageNumber, pageSize, cancellationToken);

        var transactionOutput = transactions
            .Select(t => new GetAllTransactionsByAccountIdServiceOutputTransaction(
                id: t.Id.ToString(),
                accountId: t.AccountId.ToString(),
                categoryId: t.CategoryId.ToString(),
                title: t.Title,
                description: t.Description,
                amount: t.Amount,
                date: t.Date,
                transactionType: t.TransactionType.ToString(),
                paymentMethod: t.PaymentMethod.ToString(),
                status: t.Status.ToString(),
                destination: t.Destination,
                updatedAt: t.UpdatedAt,
                createdAt: t.CreatedAt))
            .ToArray();

        var totalTransactions = await _transactionRepository.GetTransactionsCountAsync(accountId.Id, cancellationToken);

        var totalPages = (int)Math.Ceiling((double)totalTransactions / pageSize);

        var output = GetAllTransactionsByAccountIdServiceOutput.Factory(
            totalTransactions: totalTransactions,
            pageNumber: pageNumber,
            pageSize: pageSize,
            totalPages: totalPages,
            transactions: transactionOutput);

        return output;
    }
}
