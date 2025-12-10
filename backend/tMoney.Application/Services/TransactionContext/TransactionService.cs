using tMoney.Application.Services.TransactionContext.Inputs;
using tMoney.Application.Services.TransactionContext.Interfaces;
using tMoney.Application.Services.TransactionContext.Outputs;
using tMoney.Domain.BoundedContexts.CategoryContext.ENUMs;
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

        if (input.TransactionType == TransactionType.Income && category.Type == CategoryType.Expense ||
            input.TransactionType == TransactionType.Expense && category.Type == CategoryType.Income)
            throw new ArgumentException($"A categoria '{category.Title}' ({category.Type}) não pode ser usada para uma transação do tipo {input.TransactionType}.");

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

    public async Task<UpdateTransactionDetailsByIdServiceOutput> UpdateTransactionDetailsByIdServiceAsync(IdValueObject transactionId, 
        IdValueObject accountId, UpdateTransactionDetailsByIdServiceInput input, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetByIdAsync(transactionId.Id, accountId.Id, cancellationToken);
        if (transaction is null)
            throw new KeyNotFoundException("Transação não encontrada");

        var account = await _accountRepository.GetAccountByIdAsync(accountId.Id, cancellationToken);
        if (account is null)
            throw new KeyNotFoundException("Conta não encontrada");

        var validateDestination = input.TransactionType == TransactionType.Income ? null : input.Destination;

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var oldAmount = transaction.Amount;
            var oldTransactionType = transaction.TransactionType;
            var oldStatus = transaction.Status;

            if (oldStatus == PaymentStatus.Paid)
                if (oldTransactionType == TransactionType.Income)
                    account.DecrementBalance(oldAmount);
                else
                    account.IncrementBalance(oldAmount);

            transaction.UpdateTransactionDetails(
                categoryId: input.CategoryId,
                title: input.Title,
                description: input.Description,
                amount: input.Amount,
                date: input.Date,
                transactionType: input.TransactionType,
                paymentMethod: input.PaymentMethod,
                status: input.Status,
                destination: validateDestination);

            if (transaction.Status == PaymentStatus.Paid)
                if (transaction.TransactionType == TransactionType.Income)
                    account.IncrementBalance(transaction.Amount);
                else
                    account.DecrementBalance(transaction.Amount);

            _transactionRepository.Update(transaction);

            _accountRepository.Update(account);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            var output = UpdateTransactionDetailsByIdServiceOutput.Factory(
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

    public async Task DeleteTransactionByIdServiceAsync(
        IdValueObject transactionId, 
        IdValueObject accountId, 
        CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetByIdAsync(transactionId.Id, accountId.Id, cancellationToken);
        if (transaction is null)
            throw new KeyNotFoundException("Transação não encontrada");

        var account = await _accountRepository.GetAccountByIdAsync(accountId.Id, cancellationToken);
        if (account is null)
            throw new KeyNotFoundException("Conta não encontrada");

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            if (transaction.Status == PaymentStatus.Paid)
                if (transaction.TransactionType == TransactionType.Income)
                    account.DecrementBalance(transaction.Amount);
                else
                    account.IncrementBalance(transaction.Amount);

            _transactionRepository.Delete(transaction);

            _accountRepository.Update(account);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<GetFinancialSummaryServiceOutput> GetFinancialSummaryServiceAsync(
        IdValueObject accountId, 
        DateTime? startDate, 
        DateTime? endDate, 
        CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetAccountByIdAsync(accountId.Id, cancellationToken);
        if (account is null)
            throw new KeyNotFoundException("Conta não encontrada");

        var start = startDate ?? new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
        var end = endDate ?? start.AddMonths(1).AddDays(-1);

        var (income, expense) = await _transactionRepository.GetFinancialSummaryAsync(accountId.Id, start, end, cancellationToken);

        var balance = income - expense;

        var output = GetFinancialSummaryServiceOutput.Factory(
            totalIncome: income,
            totalExpense: expense,
            balance: balance,
            startDate: start,
            endDate: end);

        return output;
    }
}
