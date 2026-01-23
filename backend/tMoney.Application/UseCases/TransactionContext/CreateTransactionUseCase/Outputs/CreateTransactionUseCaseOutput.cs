namespace tMoney.Application.UseCases.TransactionContext.CreateTransactionUseCase.Outputs;

public sealed class CreateTransactionUseCaseOutput
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
    public CreateTransactionUseCaseOutputInstallment? Installment { get; }
    public DateTime? UpdatedAt { get; }
    public DateTime CreatedAt { get; }

    private CreateTransactionUseCaseOutput(string id, string accountId, string cardId, string categoryId, string title, string? description, decimal amount, 
        DateTime date, string transactionType, string paymentMethod, string status, string? destination, CreateTransactionUseCaseOutputInstallment? installment, 
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

    public static CreateTransactionUseCaseOutput Factory(string id, string accountId, string cardId, string categoryId, string title, string? description, 
        decimal amount, DateTime date, string transactionType, string paymentMethod, string status, string? destination, 
        CreateTransactionUseCaseOutputInstallment? installment, DateTime? updatedAt, DateTime createdAt)
        => new(id, accountId, cardId, categoryId, title, description, amount, date, transactionType, paymentMethod, status, destination, installment, updatedAt, 
            createdAt);
}

public sealed class CreateTransactionUseCaseOutputInstallment
{
    public string Id { get; }
    public int TotalInstallments { get; }
    public decimal TotalAmount { get; }
    public CreateTransactionUseCaseOutputInstallmentItem[] Items { get; }
    public DateTime? UpdatedAt { get; }
    public DateTime CreatedAt { get; }

    private CreateTransactionUseCaseOutputInstallment(string id, int totalInstallments, decimal totalAmount, 
        CreateTransactionUseCaseOutputInstallmentItem[] items, DateTime? updatedAt, DateTime createdAt)
    {
        Id = id;
        TotalInstallments = totalInstallments;
        TotalAmount = totalAmount;
        Items = items;
        UpdatedAt = updatedAt;
        CreatedAt = createdAt;
    }

    public static CreateTransactionUseCaseOutputInstallment Factory(string id, int totalInstallments, decimal totalAmount,
        CreateTransactionUseCaseOutputInstallmentItem[] items, DateTime? updatedAt, DateTime createdAt)
        => new(id, totalInstallments, totalAmount, items, updatedAt, createdAt);
}

public sealed class CreateTransactionUseCaseOutputInstallmentItem
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

    private CreateTransactionUseCaseOutputInstallmentItem(string id, string? invoiceId, int number, decimal amount, DateTime dueDate, string status, 
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

    public static CreateTransactionUseCaseOutputInstallmentItem Factory(string id, string? invoiceId, int number, decimal amount, DateTime dueDate,
        string status, DateTime? paidAt, DateTime? updatedAt, DateTime createdAt)
        => new(id, invoiceId, number, amount, dueDate, status, paidAt, updatedAt, createdAt);
}
