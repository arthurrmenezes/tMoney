namespace tMoney.Application.UseCases.TransactionContext.CreateTransactionUseCase.Outputs;

public sealed class CreateTransactionUseCaseOutput
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
    public DateTime? UpdatedAt { get; }
    public DateTime CreatedAt { get; }
    public CreateTransactionUseCaseOutputInstallment? Installment { get; }

    private CreateTransactionUseCaseOutput(string id, string accountId, string categoryId, string title, string? description, decimal amount, 
        DateTime date, string transactionType, string paymentMethod, string status, string? destination, DateTime? updatedAt, DateTime createdAt, 
        CreateTransactionUseCaseOutputInstallment? installment)
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
        UpdatedAt = updatedAt;
        CreatedAt = createdAt;
        Installment = installment;
    }

    public static CreateTransactionUseCaseOutput Factory(string id, string accountId, string categoryId, string title, string? description, decimal amount, 
        DateTime date, string transactionType, string paymentMethod, string status, string? destination, DateTime? updatedAt, DateTime createdAt, 
        CreateTransactionUseCaseOutputInstallment? installment)
        => new(id, accountId, categoryId, title, description, amount, date, transactionType, paymentMethod, status, destination, updatedAt, 
            createdAt, installment);
}

public sealed class CreateTransactionUseCaseOutputInstallment
{
    public string Id { get; }
    public int TotalInstallments { get; }
    public decimal TotalAmount { get; }
    public CreateTransactionUseCaseOutputInstallmentItem[] Items { get; }
    public DateTime? UpdatedAt { get; }
    public DateTime CreatedAt { get; }

    public CreateTransactionUseCaseOutputInstallment(string id, int totalInstallments, decimal totalAmount, 
        CreateTransactionUseCaseOutputInstallmentItem[] items, DateTime? updatedAt, DateTime createdAt)
    {
        Id = id;
        TotalInstallments = totalInstallments;
        TotalAmount = totalAmount;
        Items = items;
        UpdatedAt = updatedAt;
        CreatedAt = createdAt;
    }
}

public sealed class CreateTransactionUseCaseOutputInstallmentItem
{
    public string Id { get; }
    public int Number { get; }
    public decimal Amount { get; }
    public DateTime DueDate { get; }
    public string Status { get; }
    public DateTime? PaidAt { get; }
    public DateTime? UpdatedAt { get; }
    public DateTime CreatedAt { get; }

    public CreateTransactionUseCaseOutputInstallmentItem(string id, int number, decimal amount, DateTime dueDate, string status, DateTime? paidAt, 
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
