using tMoney.Domain.BoundedContexts.TransactionContext.ENUMs;
using tMoney.Domain.ValueObjects;

namespace tMoney.Application.UseCases.TransactionContext.UpdateTransactionUseCase.Input;

public sealed class UpdateTransactionUseCaseInput
{
    public IdValueObject TransactionId { get; }
    public IdValueObject AccountId { get; }
    public IdValueObject? CategoryId { get; }
    public string? Title { get; }
    public string? Description { get; }
    public decimal? Amount { get; }
    public DateTime? Date { get; }
    public PaymentStatus? Status { get; }
    public string? Destination { get; }

    private UpdateTransactionUseCaseInput(IdValueObject transactionId, IdValueObject accountId, IdValueObject? categoryId, string? title, string? description, 
        decimal? amount, DateTime? date, PaymentStatus? status, string? destination)
    {
        TransactionId = transactionId;
        AccountId = accountId;
        CategoryId = categoryId;
        Title = title;
        Description = description;
        Amount = amount;
        Date = date;
        Status = status;
        Destination = destination;
    }

    public static UpdateTransactionUseCaseInput Factory(IdValueObject transactionId, IdValueObject accountId, IdValueObject? categoryId, string? title, 
        string? description, decimal? amount, DateTime? date, PaymentStatus? status, string? destination)
        => new(transactionId, accountId, categoryId, title, description, amount, date, status, destination);
}
