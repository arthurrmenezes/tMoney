namespace tMoney.Application.Services.TransactionContext.Outputs;

public sealed class GetTransactionByIdServiceOutput
{
    public string Id { get; private set; }
    public string AccountId { get; private set; }
    public string CategoryId { get; private set; }
    public string Title { get; private set; }
    public string? Description { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime Date { get; private set; }
    public string TransactionType { get; private set; }
    public string PaymentMethod { get; private set; }
    public string Status { get; private set; }
    public string? Destination { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private GetTransactionByIdServiceOutput(string id, string accountId, string categoryId, string title, string? description,
        decimal amount, DateTime date, string transactionType, string paymentMethod, string status, string? destination,
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
        UpdatedAt = updatedAt;
        CreatedAt = createdAt;
    }

    public static GetTransactionByIdServiceOutput Factory(string id, string accountId, string categoryId, string title, string? description,
        decimal amount, DateTime date, string transactionType, string paymentMethod, string status, string? destination,
        DateTime? updatedAt, DateTime createdAt)
        => new(id, accountId, categoryId, title, description, amount, date, transactionType, paymentMethod, status, destination, updatedAt, createdAt);
}
