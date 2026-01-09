namespace tMoney.Application.UseCases.TransactionContext.GetAllTransactionsUseCase.Outputs;

public sealed class GetAllTransactionsUseCaseOutput
{
    public int TotalTransactions { get; }
    public int PageNumber { get; }
    public int PageSize { get; }
    public int TotalPages { get; }
    public GetAllTransactionsUseCaseOutputTransaction[] Transactions { get; }

    private GetAllTransactionsUseCaseOutput(int totalTransactions, int pageNumber, int pageSize, int totalPages,
        GetAllTransactionsUseCaseOutputTransaction[] transactions)
    {
        TotalTransactions = totalTransactions;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalPages = totalPages;
        Transactions = transactions;
    }

    public static GetAllTransactionsUseCaseOutput Factory(int totalTransactions, int pageNumber, int pageSize, int totalPages,
        GetAllTransactionsUseCaseOutputTransaction[] transactions)
        => new(totalTransactions, pageNumber, pageSize, totalPages, transactions);
}

public class GetAllTransactionsUseCaseOutputTransaction
{
    public string Id { get; }
    public string AccountId { get; }
    public string CategoryId { get; }
    public string Title { get; }
    public string? Description { get; }
    public decimal Amount { get; }
    public DateTime Date { get; }
    public string TransactionType { get; }
    public string PaymentMethod { get; }
    public string Status { get; }
    public string? Destination { get; }
    public GetAllTransactionsUseCaseOutputInstallment[]? Installments { get; }
    public DateTime? UpdatedAt { get; }
    public DateTime CreatedAt { get; }

    public GetAllTransactionsUseCaseOutputTransaction(string id, string accountId, string categoryId, string title, string? description, decimal amount, 
        DateTime date, string transactionType, string paymentMethod, string status, string? destination, 
        GetAllTransactionsUseCaseOutputInstallment[] installments, DateTime? updatedAt, DateTime createdAt)
    {
        Id = id;
        AccountId = accountId;
        CategoryId = categoryId;
        Title = title;
        Description = description;
        Amount = amount;
        Date = date;
        TransactionType = transactionType;
        PaymentMethod = paymentMethod;
        Status = status;
        Destination = destination;
        Installments = installments;
        UpdatedAt = updatedAt;
        CreatedAt = createdAt;
    }
}

public sealed class GetAllTransactionsUseCaseOutputInstallment
{
    public string Id { get; }
    public int TotalInstallments { get; }
    public decimal TotalAmount { get; }
    public DateTime FirstPaymentDate { get; }
    public string Status { get; }
    public GetAllTransactionsUseCaseOutputInstallmentItem[] InstallmentItems { get; }

    public GetAllTransactionsUseCaseOutputInstallment(string id, int totalInstallments, decimal totalAmount, DateTime firstPaymentDate, string status, 
        GetAllTransactionsUseCaseOutputInstallmentItem[] installmentItems)
    {
        Id = id;
        TotalInstallments = totalInstallments;
        TotalAmount = totalAmount;
        FirstPaymentDate = firstPaymentDate;
        Status = status;
        InstallmentItems = installmentItems;
    }
}

public sealed class GetAllTransactionsUseCaseOutputInstallmentItem
{
    public string Id { get; }
    public int Number { get; }
    public decimal Amount { get; }
    public DateTime DueDate { get; }
    public string Status { get; }
    public DateTime? PaidAt { get; }
    public DateTime? UpdatedAt { get; }
    public DateTime CreatedAt { get; }

    public GetAllTransactionsUseCaseOutputInstallmentItem(string id, int number, decimal amount, DateTime dueDate, string status, DateTime? paidAt,
        DateTime? updatedAt, DateTime createdAt)
    {
        Id = id;
        Number = number;
        Amount = amount;
        DueDate = dueDate;
        Status = status;
        PaidAt = paidAt;
        UpdatedAt = updatedAt;
        CreatedAt = createdAt;
    }
}
