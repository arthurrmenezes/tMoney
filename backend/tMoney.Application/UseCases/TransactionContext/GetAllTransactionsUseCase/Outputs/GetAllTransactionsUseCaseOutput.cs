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

public sealed class GetAllTransactionsUseCaseOutputTransaction
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
    public string TransactionType { get; }
    public string PaymentMethod { get; }
    public string Status { get; }
    public string? Destination { get; }
    public GetAllTransactionsUseCaseOutputInstallment[]? Installments { get; }
    public DateTime? UpdatedAt { get; }
    public DateTime CreatedAt { get; }

    private GetAllTransactionsUseCaseOutputTransaction(string id, string accountId, string cardId, string categoryId, string? installmentId, string? invoiceId, 
        string title, string? description, decimal amount, DateTime date, string transactionType, string paymentMethod, string status, string? destination, 
        GetAllTransactionsUseCaseOutputInstallment[]? installments, DateTime? updatedAt, DateTime createdAt)
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
        TransactionType = transactionType;
        PaymentMethod = paymentMethod;
        Status = status;
        Destination = destination;
        Installments = installments;
        UpdatedAt = updatedAt;
        CreatedAt = createdAt;
    }

    public static GetAllTransactionsUseCaseOutputTransaction Factory(string id, string accountId, string cardId, string categoryId, string? installmentId,
        string? invoiceId, string title, string? description, decimal amount, DateTime date, string transactionType, string paymentMethod, string status,
        string? destination, GetAllTransactionsUseCaseOutputInstallment[]? installments, DateTime? updatedAt, DateTime createdAt)
        => new(id, accountId, cardId, categoryId, installmentId, invoiceId, title, description, amount, date, transactionType, paymentMethod, status, destination,
            installments, updatedAt, createdAt);
}

public sealed class GetAllTransactionsUseCaseOutputInstallment
{
    public string Id { get; }
    public int TotalInstallments { get; }
    public decimal TotalAmount { get; }
    public DateTime FirstPaymentDate { get; }
    public string Status { get; }
    public GetAllTransactionsUseCaseOutputInstallmentItem[] InstallmentItems { get; }

    private GetAllTransactionsUseCaseOutputInstallment(string id, int totalInstallments, decimal totalAmount, DateTime firstPaymentDate, string status, 
        GetAllTransactionsUseCaseOutputInstallmentItem[] installmentItems)
    {
        Id = id;
        TotalInstallments = totalInstallments;
        TotalAmount = totalAmount;
        FirstPaymentDate = firstPaymentDate;
        Status = status;
        InstallmentItems = installmentItems;
    }

    public static GetAllTransactionsUseCaseOutputInstallment Factory(string id, int totalInstallments, decimal totalAmount, DateTime firstPaymentDate,
        string status, GetAllTransactionsUseCaseOutputInstallmentItem[] installmentItems)
        => new(id, totalInstallments, totalAmount, firstPaymentDate, status, installmentItems);
}

public sealed class GetAllTransactionsUseCaseOutputInstallmentItem
{
    public string Id { get; }
    public string? InvoiceId { get; }
    public int Number { get; }
    public decimal Amount { get; }
    public DateTime DueDate { get; }
    public string Status { get; }
    public DateTime? PaidAt { get; }
    public DateTime? UpdatedAt { get; }
    public DateTime CreatedAt { get; }

    private GetAllTransactionsUseCaseOutputInstallmentItem(string id, string? invoiceId, int number, decimal amount, DateTime dueDate, string status, 
        DateTime? paidAt, DateTime? updatedAt, DateTime createdAt)
    {
        Id = id;
        InvoiceId = invoiceId;
        Number = number;
        Amount = amount;
        DueDate = dueDate;
        Status = status;
        PaidAt = paidAt;
        UpdatedAt = updatedAt;
        CreatedAt = createdAt;
    }

    public static GetAllTransactionsUseCaseOutputInstallmentItem Factory(string id, string? invoiceId, int number, decimal amount, DateTime dueDate,
        string status, DateTime? paidAt, DateTime? updatedAt, DateTime createdAt)
        => new(id, invoiceId, number, amount, dueDate, status, paidAt, updatedAt, createdAt);
}
