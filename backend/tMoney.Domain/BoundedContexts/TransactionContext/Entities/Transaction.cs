using tMoney.Domain.BoundedContexts.TransactionContext.ENUMs;
using tMoney.Domain.ValueObjects;

namespace tMoney.Domain.BoundedContexts.TransactionContext.Entities;

public class Transaction
{
    public IdValueObject Id { get; private set; }
    public IdValueObject AccountId { get; private set; }
    public IdValueObject CategoryId { get; private set; }
    public string Title { get; private set; }
    public string? Description { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime Date { get; private set; }
    public TransactionType TransactionType { get; private set; }
    public PaymentMethod PaymentMethod { get; private set; }
    public PaymentStatus Status { get; private set; }
    public string? Destination { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Transaction(IdValueObject accountId, IdValueObject categoryId, string title, string? description, decimal amount, DateTime date, 
        TransactionType transactionType, PaymentMethod paymentMethod, PaymentStatus status, string? destination)
    {
        ValidateDomain(title, description, amount, transactionType, paymentMethod, status, destination);

        Id = IdValueObject.New();
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
        UpdatedAt = null;
        CreatedAt = DateTime.UtcNow;
    }

    private void ValidateDomain(string title, string? description, decimal amount, TransactionType transactionType, PaymentMethod paymentMethod, 
        PaymentStatus status, string? destination)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("O título não pode ser nulo ou vazio.");
        if (title.Length > 50)
            throw new ArgumentException("O título não pode ultrapassar 50 caracteres.");

        if (!string.IsNullOrWhiteSpace(description))
            if (description.Length > 300)
                throw new ArgumentException("A descrição não pode ultrapassar 300 caracteres");

        if (amount <= 0)
            throw new ArgumentException("Valor inválido.");

        if (!Enum.IsDefined(typeof(TransactionType), transactionType))
            throw new ArgumentException("Tipo de transação inválido.");

        if (!Enum.IsDefined(typeof(PaymentMethod), paymentMethod))
            throw new ArgumentException("Tipo de transação inválido.");

        if (!Enum.IsDefined(typeof(PaymentStatus), status))
            throw new ArgumentException("Status de pagamento inválido.");

        if (!string.IsNullOrWhiteSpace(destination))
            if (destination!.Length > 50)
                throw new ArgumentException("O valor do destino não pode ultrapassar 50 caracteres");
    }
}
