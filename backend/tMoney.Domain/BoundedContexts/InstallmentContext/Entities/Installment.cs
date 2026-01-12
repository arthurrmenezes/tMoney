using tMoney.Domain.BoundedContexts.TransactionContext.ENUMs;
using tMoney.Domain.ValueObjects;

namespace tMoney.Domain.BoundedContexts.InstallmentContext.Entities;

public class Installment
{
    public IdValueObject Id { get; private set; }
    public IdValueObject AccountId { get; private set; }
    public int TotalInstallments { get; private set; }
    public decimal TotalAmount { get; private set; }
    public DateTime FirstPaymentDate { get; private set; }
    public PaymentStatus Status { get; private set; }
    public List<InstallmentItem> Installments { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }

    protected Installment() { }

    public Installment(IdValueObject accountId, int totalInstallments, decimal totalAmount, DateTime firstPaymentDate, PaymentStatus status)
    {
        ValidateDomain(totalInstallments, totalAmount, status);

        Id = IdValueObject.New();
        AccountId = accountId;
        TotalInstallments = totalInstallments;
        TotalAmount = totalAmount;
        FirstPaymentDate = firstPaymentDate;
        Status = status;
        Installments = new List<InstallmentItem>(); ;
        UpdatedAt = null;
        CreatedAt = DateTime.UtcNow;
    }

    private void ValidateDomain(int totalInstallments, decimal totalAmount, PaymentStatus status)
    {
        if (totalInstallments <= 0 || totalInstallments > 480)
            throw new ArgumentException("Número de parcelas inválido.");

        if (totalAmount <= 0)
            throw new ArgumentException("Valor inválido.");

        if (!Enum.IsDefined(typeof(PaymentStatus), status))
            throw new ArgumentException("Status da parcela inválido.");
    }

    public void GenerateInstallments()
    {
        var amount = Math.Floor(TotalAmount / TotalInstallments * 100) / 100;

        var amountDifference = TotalAmount - (amount * TotalInstallments);

        for (int i = 1; i <= TotalInstallments; i++)
        {
            var finalAmount = (i == TotalInstallments) ? amount + amountDifference : amount;

            var dueDate = FirstPaymentDate.AddMonths(i - 1);

            var installmentItem = new InstallmentItem(
                installmentId: Id,
                number: i,
                amount: finalAmount,
                dueDate: dueDate);

            Installments.Add(installmentItem);
        }
    }

    public void PayAllInstallments()
    {
        if (Installments.Count() == 0)
            throw new ArgumentException("Não há parcelas registradas.");

        Installments.ForEach(i => i.PayInstallment());

        UpdatedAt = DateTime.UtcNow;
    }
    
    public void UpdateInstallmentDetails(int? totalInstallments, decimal? totalAmount, DateTime? firstPaymentDate, PaymentStatus? status)
    {
        if (Status == PaymentStatus.Paid && status.HasValue && status != PaymentStatus.Paid)
            throw new InvalidOperationException("Não é possível reabrir um parcelamento já quitado.");

        var isStructuralChange =
            (totalInstallments.HasValue && totalInstallments != TotalInstallments) ||
            (totalAmount.HasValue && totalAmount != TotalAmount) ||
            (firstPaymentDate.HasValue && firstPaymentDate != FirstPaymentDate);

        var hasPaidItems = Installments.Any(i => i.Status == PaymentStatus.Paid);

        if (isStructuralChange && hasPaidItems)
            throw new ArgumentException("Não foi possível alterar os dados pois já existem parcelas pagas.");

        if (totalInstallments.HasValue)
            TotalInstallments = totalInstallments.Value;

        if (totalAmount.HasValue)
            TotalAmount = totalAmount.Value;

        if (firstPaymentDate.HasValue)
            FirstPaymentDate = firstPaymentDate.Value;

        if (status.HasValue && Enum.IsDefined(typeof(PaymentStatus), status))
            Status = status.Value;

        if (isStructuralChange)
        {
            Installments.Clear();
            GenerateInstallments();
        }

        if ((status.HasValue && status.Value == PaymentStatus.Paid) || 
            Status == PaymentStatus.Paid || 
            (isStructuralChange && Status == PaymentStatus.Paid))
            PayAllInstallments();

        UpdatedAt = DateTime.UtcNow;
    }
}
