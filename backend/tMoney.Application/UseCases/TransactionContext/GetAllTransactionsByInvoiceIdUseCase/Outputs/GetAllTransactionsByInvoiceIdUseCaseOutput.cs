namespace tMoney.Application.UseCases.TransactionContext.GetAllTransactionsByInvoiceIdUseCase.Outputs;

public sealed class GetAllTransactionsByInvoiceIdUseCaseOutput
{
    public int TotalTransactions { get; }
    public int PageNumber { get; }
    public int PageSize { get; }
    public int TotalPages { get; }
    public GetAllTransactionsByInvoiceIdUseCaseOutputTransaction[] Transactions { get; }

    private GetAllTransactionsByInvoiceIdUseCaseOutput(int totalTransactions, int pageNumber, int pageSize, int totalPages,
        GetAllTransactionsByInvoiceIdUseCaseOutputTransaction[] transactions)
    {
        TotalTransactions = totalTransactions;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalPages = totalPages;
        Transactions = transactions;
    }

    public static GetAllTransactionsByInvoiceIdUseCaseOutput Factory(int totalTransactions, int pageNumber, int pageSize, int totalPages,
        GetAllTransactionsByInvoiceIdUseCaseOutputTransaction[] transactions)
        => new(totalTransactions, pageNumber, pageSize, totalPages, transactions);
}

public sealed class GetAllTransactionsByInvoiceIdUseCaseOutputTransaction
{
    public string Id { get; }
    public string AccountId { get; }
    public string CardId { get; }
    public string CategoryId { get; }
    public string? InstallmentId { get; }
    public string? InvoiceId { get; }
    public string Title { get; }
    public string? Description { get; }
    public decimal Amount { get; }
    public DateTime Date { get; }
    public DateTime DueDate { get; }
    public int CurrentNumber { get; }
    public int TotalInstallments { get; }
    public string TransactionType { get; }
    public string PaymentMethod { get; }
    public string Status { get; }
    public string? Destination { get; }
    public DateTime? PaidAt { get; }
    public DateTime? UpdatedAt { get; }
    public DateTime CreatedAt { get; }

    private GetAllTransactionsByInvoiceIdUseCaseOutputTransaction(string id, string accountId, string cardId, string categoryId, string? installmentId, 
        string? invoiceId, string title, string? description, decimal amount, DateTime date, DateTime dueDate, int currentNumber, int totalInstallments, 
        string transactionType, string paymentMethod, string status, string? destination, DateTime? paidAt, DateTime? updatedAt, DateTime createdAt)
    {
        Id = id;
        AccountId = accountId;
        CardId = cardId;
        CategoryId = categoryId;
        InstallmentId = installmentId;
        InvoiceId = invoiceId;
        Title = title;
        Description = description;
        Amount = amount;
        Date = date;
        DueDate = dueDate;
        CurrentNumber = currentNumber;
        TotalInstallments = totalInstallments;
        TransactionType = transactionType;
        PaymentMethod = paymentMethod;
        Status = status;
        Destination = destination;
        PaidAt = paidAt;
        UpdatedAt = updatedAt;
        CreatedAt = createdAt;
    }

    public static GetAllTransactionsByInvoiceIdUseCaseOutputTransaction Factory(string id, string accountId, string cardId, string categoryId,
        string? installmentId, string? invoiceId, string title, string? description, decimal amount, DateTime date, DateTime dueDate, int currentNumber, 
        int totalInstallments, string transactionType, string paymentMethod, string status, string? destination, DateTime? paidAt, DateTime? updatedAt, 
        DateTime createdAt)
        => new(id, accountId, cardId, categoryId, installmentId, invoiceId, title, description, amount, date, dueDate, currentNumber, totalInstallments, 
            transactionType, paymentMethod, status, destination, paidAt, updatedAt, createdAt);
}
