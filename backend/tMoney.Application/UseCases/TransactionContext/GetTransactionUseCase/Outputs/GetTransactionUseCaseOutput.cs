namespace tMoney.Application.UseCases.TransactionContext.GetTransactionUseCase.Outputs;

public sealed class GetTransactionUseCaseOutput
{
    public string Id { get; }
    public string AccountId { get; }
    public string CardId { get; }
    public string CategoryId { get; }
    public string Title { get; }
    public string? Description { get; }
    public decimal Amount { get; }
    public DateTime Date { get; }
    public string TransactionType { get; }
    public string PaymentMethod { get; }
    public string Status { get; }
    public string? Destination { get; }
    public GetTransactionUseCaseOutputInstallment? Installment { get; }
    public DateTime? UpdatedAt { get; }
    public DateTime CreatedAt { get; }

    private GetTransactionUseCaseOutput(string id, string accountId, string cardId, string categoryId, string title, string? description, decimal amount, 
        DateTime date, string transactionType, string paymentMethod, string status, string? destination, GetTransactionUseCaseOutputInstallment? installment, 
        DateTime? updatedAt, DateTime createdAt)
    {
        Id = id;
        AccountId = accountId;
        CardId = cardId;
        CategoryId = categoryId;
        Title = title;
        Description = description;
        Amount = amount;
        Date = date;
        TransactionType = transactionType;
        PaymentMethod = paymentMethod;
        Status = status;
        Destination = destination;
        Installment = installment;
        UpdatedAt = updatedAt;
        CreatedAt = createdAt;
    }

    public static GetTransactionUseCaseOutput Factory(string id, string accountId, string cardId, string categoryId, string title, string? description, 
        decimal amount, DateTime date, string transactionType, string paymentMethod, string status, string? destination, 
        GetTransactionUseCaseOutputInstallment? installment, DateTime? updatedAt, DateTime createdAt)
        => new(id, accountId, cardId, categoryId, title, description, amount, date, transactionType, paymentMethod, status, destination, installment, updatedAt, 
            createdAt);
}

public sealed class GetTransactionUseCaseOutputInstallment
{
    public string Id { get; }
    public int TotalInstallments { get; }
    public decimal TotalAmount { get; }
    public DateTime FirstPaymentDate { get; }
    public string Status { get; }
    public GetTransactionUseCaseOutputInstallmentItem[] Installments { get; }

    private GetTransactionUseCaseOutputInstallment(string id, int totalInstallments, decimal totalAmount, DateTime firstPaymentDate, string status, 
        GetTransactionUseCaseOutputInstallmentItem[] installments)
    {
        Id = id;
        TotalInstallments = totalInstallments;
        TotalAmount = totalAmount;
        FirstPaymentDate = firstPaymentDate;
        Status = status;
        Installments = installments;
    }

    public static GetTransactionUseCaseOutputInstallment Factory(string id, int totalInstallments, decimal totalAmount, DateTime firstPaymentDate,
        string status, GetTransactionUseCaseOutputInstallmentItem[] installments)
        => new(id, totalInstallments, totalAmount, firstPaymentDate, status, installments);
}

public sealed class GetTransactionUseCaseOutputInstallmentItem
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

    private GetTransactionUseCaseOutputInstallmentItem(string id, string? invoiceId, int number, decimal amount, DateTime dueDate, string status, 
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

    public static GetTransactionUseCaseOutputInstallmentItem Factory(string id, string? invoiceId, int number, decimal amount, DateTime dueDate, string status,
        DateTime? paidAt, DateTime? updatedAt, DateTime createdAt)
        => new(id, invoiceId, number, amount, dueDate, status, paidAt, updatedAt, createdAt);
}
