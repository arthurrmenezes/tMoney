namespace tMoney.Application.UseCases.TransactionContext.UpdateTransactionUseCase.Outputs;

public sealed class UpdateTransactionUseCaseOutput
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
    public UpdateTransactionUseCaseOutputInstallment? Installment { get; }
    public DateTime? UpdatedAt { get; }
    public DateTime CreatedAt { get; }

    private UpdateTransactionUseCaseOutput(string id, string accountId, string categoryId, string title, string? description, decimal amount, DateTime date, 
        string transactionType, string paymentMethod, string status, string? destination, UpdateTransactionUseCaseOutputInstallment? installment,
        DateTime? updatedAt, DateTime createdAt)
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
        Installment = installment;
        UpdatedAt = updatedAt;
        CreatedAt = createdAt;
    }

    public static UpdateTransactionUseCaseOutput Factory(string id, string accountId, string categoryId, string title, string? description, decimal amount, 
        DateTime date, string transactionType, string paymentMethod, string status, string? destination, UpdateTransactionUseCaseOutputInstallment? installment, 
        DateTime? updatedAt, DateTime createdAt)
        => new(id, accountId, categoryId, title, description, amount, date, transactionType, paymentMethod, status, destination, installment, updatedAt, 
            createdAt);
}

public sealed class UpdateTransactionUseCaseOutputInstallment
{
    public string Id { get; }
    public int TotalInstallments { get; }
    public UpdateTransactionUseCaseOutputInstallmentItem[] Installments { get; }

    public UpdateTransactionUseCaseOutputInstallment(string id, int totalInstallments, UpdateTransactionUseCaseOutputInstallmentItem[] installments)
    {
        Id = id;
        TotalInstallments = totalInstallments;
        Installments = installments;
    }
}

public sealed class UpdateTransactionUseCaseOutputInstallmentItem
{
    public string Id { get; }
    public int Number { get; }
    public decimal Amount { get; }
    public DateTime DueDate { get; }
    public string Status { get; }
    public DateTime? PaidAt { get; }
    public DateTime? UpdatedAt { get; }
    public DateTime CreatedAt { get; }

    public UpdateTransactionUseCaseOutputInstallmentItem(string id, int number, decimal amount, DateTime dueDate, string status, DateTime? paidAt,
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
