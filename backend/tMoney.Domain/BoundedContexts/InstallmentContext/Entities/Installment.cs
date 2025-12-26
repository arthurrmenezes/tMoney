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
            throw new ArgumentException($"Não há parcelas registradas.");

        Installments.ForEach(i => i.PayAllInstallments());
    }

    public void PaySingleInstallmentItem(int number, DateTime paidAt)
    {
        var installmentItem = Installments.FirstOrDefault(i => i.Number == number);
        if (installmentItem is null)
            throw new ArgumentException($"Não foi encontrado nenhuma parcela de número {number}.");

        if (installmentItem.Status == PaymentStatus.Paid)
            throw new InvalidOperationException($"A parcela {number} já foi paga.");

        installmentItem.UpdateInstallmentItem(PaymentStatus.Paid, paidAt);
    }
}
