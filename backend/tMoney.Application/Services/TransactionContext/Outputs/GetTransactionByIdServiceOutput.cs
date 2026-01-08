namespace tMoney.Application.Services.TransactionContext.Outputs;

public sealed class GetTransactionByIdServiceOutput
{
    public string Id { get; }
    public string AccountId { get; }
    public string CategoryId { get; }
    public string? InstallmentId { get; }
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

    private GetTransactionByIdServiceOutput(string id, string accountId, string categoryId, string? installmentId, string title, string? description,
        decimal amount, DateTime date, string transactionType, string paymentMethod, string status, string? destination,
        DateTime? updatedAt, DateTime createdAt)
    {
        Id = id;
        AccountId = accountId;
        CategoryId = categoryId;
        InstallmentId = installmentId;
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
    }

    public static GetTransactionByIdServiceOutput Factory(string id, string accountId, string categoryId, string? installmentId, string title, 
        string? description, decimal amount, DateTime date, string transactionType, string paymentMethod, string status, string? destination,
        DateTime? updatedAt, DateTime createdAt)
        => new(id, accountId, categoryId, installmentId, title, description, amount, date, transactionType, paymentMethod, status, destination, updatedAt, 
            createdAt);
}
