using tMoney.Application.Services.TransactionContext.Inputs;
using tMoney.Application.Services.TransactionContext.Interfaces;
using tMoney.Application.Services.TransactionContext.Outputs;
using tMoney.Domain.BoundedContexts.CategoryContext.ENUMs;
using tMoney.Domain.BoundedContexts.TransactionContext.Entities;
using tMoney.Domain.BoundedContexts.TransactionContext.ENUMs;
using tMoney.Domain.ValueObjects;
using tMoney.Infrastructure.Data.Repositories.Interfaces;

namespace tMoney.Application.Services.TransactionContext;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ICategoryRepository _categoryRepository;

    public TransactionService(ITransactionRepository transactionRepository, IAccountRepository accountRepository, 
        ICategoryRepository categoryRepository)
    {
        _transactionRepository = transactionRepository;
        _accountRepository = accountRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<CreateTransactionServiceOutput> CreateTransactionServiceAsync(
        CreateTransactionServiceInput input, 
        CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(input.CategoryId.Id, input.AccountId.Id, cancellationToken);
        if (category is null)
            throw new KeyNotFoundException("Categoria não encontrada");

        if (input.TransactionType == TransactionType.Income && category.Type == CategoryType.Expense ||
            input.TransactionType == TransactionType.Expense && category.Type == CategoryType.Income)
            throw new ArgumentException($"A categoria '{category.Title}' ({category.Type.ToString()}) não pode ser usada para uma transação do tipo {input.TransactionType.ToString()}.");

        var transaction = new Transaction(
            accountId: input.AccountId,
            cardId: input.CardId,
            categoryId: input.CategoryId,
            installmentId: input.InstallmentId,
            title: input.Title,
            description: input.Description,
            amount: input.Amount,
            date: input.Date,
            transactionType: input.TransactionType,
            paymentMethod: input.PaymentMethod,
            status: input.Status,
            destination: input.Destination);

            await _transactionRepository.AddAsync(transaction, cancellationToken);

            var output = CreateTransactionServiceOutput.Factory(
                id: transaction.Id.ToString(),
                accountId: transaction.AccountId.ToString(),
                cardId: transaction.CardId.ToString(),
                categoryId: transaction.CategoryId.ToString(),
                installmentId: transaction.InstallmentId is null ? null : transaction.InstallmentId.ToString(),
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

    public async Task<GetTransactionByIdServiceOutput> GetTransactionByIdServiceAsync(
        IdValueObject transactionId, 
        IdValueObject accountId,
        CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetByIdAsync(transactionId.Id, accountId.Id, cancellationToken);
        if (transaction is null)
            throw new KeyNotFoundException("Transação não encontrada");

        var installmentId = transaction.InstallmentId is null ? null : transaction.InstallmentId.ToString();

        var output = GetTransactionByIdServiceOutput.Factory(
            id: transaction.Id.ToString(),
            accountId: transaction.AccountId.ToString(),
            categoryId: transaction.CategoryId.ToString(),
            installmentId: installmentId,
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
        IdValueObject accountId,
        GetAllTransactionsByAccountIdServiceInput input,
        CancellationToken cancellationToken)
    {
        if (input.MinValue > input.MaxValue)
            throw new ArgumentException("O valor mínimo deve ser menor ou igual ao valor máximo.");

        var transactions = await _transactionRepository.GetAllByAccountIdAsync(
            accountId: accountId.Id,
            pageNumber: input.PageNumber,
            pageSize: input.PageSize,
            transactionType: input.TransactionType,
            categoryId: input.CategoryId is null ? null : input.CategoryId.Id,
            paymentMethod: input.PaymentMethod,
            paymentStatus: input.PaymentStatus,
            startDate: input.StartDate,
            endDate: input.EndDate,
            minValue: input.MinValue,
            maxValue: input.MaxValue,
            textSearch: input.TextSearch,
            hasInstallment: input.HasInstallment,
            cancellationToken: cancellationToken);

        var transactionOutput = transactions
            .Select(t => new GetAllTransactionsByAccountIdServiceOutputTransaction(
                id: t.Id.ToString(),
                accountId: t.AccountId.ToString(),
                categoryId: t.CategoryId.ToString(),
                installmentId: t.InstallmentId is null ? null : t.InstallmentId.ToString(),
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

        var totalTransactions = await _transactionRepository.GetTransactionsCountAsync(
            accountId: accountId.Id,
            transactionType: input.TransactionType,
            categoryId: input.CategoryId is null ? null : input.CategoryId.Id,
            paymentMethod: input.PaymentMethod,
            paymentStatus: input.PaymentStatus,
            startDate: input.StartDate,
            endDate: input.EndDate,
            minValue: input.MinValue,
            maxValue: input.MaxValue,
            textSearch: input.TextSearch,
            hasInstallment: input.HasInstallment,
            cancellationToken: cancellationToken);

        var totalPages = (int)Math.Ceiling((double)totalTransactions / input.PageSize);

        var output = GetAllTransactionsByAccountIdServiceOutput.Factory(
            totalTransactions: totalTransactions,
            pageNumber: input.PageNumber,
            pageSize: input.PageSize,
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

        var oldAmount = transaction.Amount;
        var oldTransactionType = transaction.TransactionType;
        var oldStatus = transaction.Status;

        transaction.UpdateTransactionDetails(
            categoryId: input.CategoryId,
            installmentId: input.InstallmentId,
            title: input.Title,
            description: input.Description,
            amount: input.Amount,
            date: input.Date,
            transactionType: input.TransactionType,
            paymentMethod: input.PaymentMethod,
            status: input.Status,
            destination: validateDestination);

        _transactionRepository.Update(transaction);

        _accountRepository.Update(account);

        var output = UpdateTransactionDetailsByIdServiceOutput.Factory(
            id: transaction.Id.ToString(),
            accountId: transaction.AccountId.ToString(),
            categoryId: transaction.CategoryId.ToString(),
            installmentId: transaction.InstallmentId is null ? null : transaction.InstallmentId.ToString(),
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

    public async Task<DeleteTransactionByIdServiceOutput> DeleteTransactionByIdServiceAsync(
        IdValueObject transactionId, 
        IdValueObject accountId, 
        CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetByIdAsync(transactionId.Id, accountId.Id, cancellationToken);
        if (transaction is null)
            throw new KeyNotFoundException("Transação não encontrada");

        string? installmentOutput = null;
        if (transaction.InstallmentId is not null)
            installmentOutput = transaction.InstallmentId.ToString();

        _transactionRepository.Delete(transaction);

        return DeleteTransactionByIdServiceOutput.Factory(
            installment: installmentOutput);
    }

    public async Task<GetFinancialSummaryServiceOutput> GetFinancialSummaryServiceAsync(
        IdValueObject accountId, 
        DateTime? startDate, 
        DateTime? endDate, 
        CancellationToken cancellationToken)
    {
        var start = startDate ?? new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
        var end = endDate ?? start.AddMonths(1).AddDays(-1);

        var (periodIncome, periodExpense) = await _transactionRepository.GetFinancialSummaryAsync(accountId.Id, start, end, cancellationToken);

        var balance = await _transactionRepository.GetTotalBalanceAsync(accountId.Id, cancellationToken);

        var output = GetFinancialSummaryServiceOutput.Factory(
            periodIncome: periodIncome,
            periodExpense: periodExpense,
            balance: balance,
            startDate: start,
            endDate: end);

        return output;
    }
}
