using tMoney.Domain.BoundedContexts.TransactionContext.ENUMs;
using tMoney.Domain.ValueObjects;

namespace tMoney.Application.UseCases.TransactionContext.CreateTransactionUseCase.Inputs;

public sealed class CreateTransactionUseCaseInput
{
    public IdValueObject AccountId { get; }
    public IdValueObject CardId { get; }
    public IdValueObject CategoryId { get; }
    public string Title { get; }
    public string? Description { get; }
    public decimal Amount { get; }
    public DateTime Date { get; }
    public TransactionType TransactionType { get; }
    public PaymentMethod PaymentMethod { get; }
    public PaymentStatus Status { get; }
    public string? Destination { get; }
    public CreateTransactionUseCaseInputInstallment? HasInstallment { get; }

    private CreateTransactionUseCaseInput(IdValueObject accountId, IdValueObject cardId, IdValueObject categoryId, string title, string? description, 
        decimal amount, DateTime date, TransactionType transactionType, PaymentMethod paymentMethod, PaymentStatus status, string? destination, 
        CreateTransactionUseCaseInputInstallment? hasInstallment)
    {
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
        HasInstallment = hasInstallment;
    }

    public static CreateTransactionUseCaseInput Factory(IdValueObject accountId, IdValueObject cardId, IdValueObject categoryId, string title, 
        string? description, decimal amount, DateTime date, TransactionType transactionType, PaymentMethod paymentMethod, PaymentStatus status, 
        string? destination, CreateTransactionUseCaseInputInstallment? hasInstallment)
        => new(accountId, cardId, categoryId, title, description, amount, date, transactionType, paymentMethod, status, destination, hasInstallment);
}

public sealed class CreateTransactionUseCaseInputInstallment
{
    public int TotalInstallments { get; }

    public CreateTransactionUseCaseInputInstallment(int totalInstallments)
    {
        TotalInstallments = totalInstallments;
    }
}
