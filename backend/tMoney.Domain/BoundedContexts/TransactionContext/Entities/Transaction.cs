using tMoney.Domain.BoundedContexts.TransactionContext.ENUMs;
using tMoney.Domain.ValueObjects;

namespace tMoney.Domain.BoundedContexts.TransactionContext.Entities;

public class Transaction
{
    public IdValueObject Id { get; private set; }
    public IdValueObject AccountId { get; private set; }
    public IdValueObject CardId { get; private set; }
    public IdValueObject CategoryId { get; private set; }
    public IdValueObject? InstallmentId { get; private set; }
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

    public Transaction(IdValueObject accountId, IdValueObject cardId, IdValueObject categoryId, IdValueObject? installmentId, string title, 
        string? description, decimal amount, DateTime date, TransactionType transactionType, PaymentMethod paymentMethod, PaymentStatus status, 
        string? destination)
    {
        Id = IdValueObject.New();
        AccountId = accountId;
        CardId = cardId;
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
        UpdatedAt = null;
        CreatedAt = DateTime.UtcNow;

        ValidateDomain();
    }

    private void ValidateDomain()
    {
        if (string.IsNullOrWhiteSpace(Title))
            throw new ArgumentException("O título não pode ser nulo ou vazio.");
        if (Title.Length > 50)
            throw new ArgumentException("O título não pode ultrapassar 50 caracteres.");

        if (!string.IsNullOrWhiteSpace(Description))
            if (Description.Length > 300)
                throw new ArgumentException("A descrição não pode ultrapassar 300 caracteres");

        if (Amount <= 0)
            throw new ArgumentException("Valor inválido.");

        if (Date >= DateTime.UtcNow.AddDays(1) && (Status == PaymentStatus.Paid || Status == PaymentStatus.Overdue))
            throw new ArgumentException($"Uma transação futura não pode ter o status '{Status}'.");
        if (Date < DateTime.Today && Status == PaymentStatus.Pending)
            throw new ArgumentException("Uma transação passada não pode ter o status 'Pendente'.");

        if (!Enum.IsDefined(typeof(TransactionType), TransactionType))
            throw new ArgumentException("Tipo de transação inválido.");

        if (!Enum.IsDefined(typeof(PaymentMethod), PaymentMethod))
            throw new ArgumentException("Tipo de transação inválido.");

        if (!Enum.IsDefined(typeof(PaymentStatus), Status))
            throw new ArgumentException("Status de pagamento inválido.");

        if (!string.IsNullOrWhiteSpace(Destination))
            if (Destination.Length > 50)
                throw new ArgumentException("O valor do destino não pode ultrapassar 50 caracteres");
    }

    public void UpdateTransactionDetails(IdValueObject? categoryId, IdValueObject? installmentId, string? title, string? description, decimal? amount, DateTime? date,
        TransactionType? transactionType, PaymentMethod? paymentMethod, PaymentStatus? status, string? destination)
    {
        if (categoryId is not null)
            CategoryId = categoryId;

        if (InstallmentId is null && installmentId is not null)
            InstallmentId = installmentId;

        if (!string.IsNullOrWhiteSpace(title) && title.Length <= 50)
            Title = title;

        if (!string.IsNullOrWhiteSpace(description) && description.Length <= 300)
            Description = description;

        if (amount.HasValue && amount > 0)
            Amount = amount.Value;

        if (date is not null)
            Date = date.Value;

        if (transactionType.HasValue && Enum.IsDefined(typeof(TransactionType), transactionType))
            TransactionType = transactionType.Value;

        if (paymentMethod.HasValue && Enum.IsDefined(typeof(PaymentMethod), paymentMethod))
            PaymentMethod = paymentMethod.Value;

        if (status.HasValue && Enum.IsDefined(typeof(PaymentStatus), status))
            Status = status.Value;

        if (!string.IsNullOrWhiteSpace(destination) && destination!.Length <= 50)
            Destination = destination;

        UpdatedAt = DateTime.UtcNow;

        ValidateDomain();
    }
}
