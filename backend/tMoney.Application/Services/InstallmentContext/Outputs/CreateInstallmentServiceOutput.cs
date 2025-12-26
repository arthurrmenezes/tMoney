using tMoney.Domain.ValueObjects;

namespace tMoney.Application.Services.InstallmentContext.Outputs;

public sealed class CreateInstallmentServiceOutput
{
    public IdValueObject Id { get; }
    public IdValueObject AccountId { get; }
    public int TotalInstallments { get; }
    public decimal TotalAmount { get; }
    public DateTime FirstPaymentDate { get; }
    public string Status { get; }
    public CreateInstallmentServiceOutputInstallmentItem[] InstallmentItems { get; }
    public DateTime? UpdatedAt { get; }
    public DateTime CreatedAt { get; }

    private CreateInstallmentServiceOutput(IdValueObject id, IdValueObject accountId, int totalInstallments, decimal totalAmount, DateTime firstPaymentDate, 
        string status, CreateInstallmentServiceOutputInstallmentItem[] installmentItems, DateTime? updatedAt, DateTime createdAt)
    {
        Id = id;
        AccountId = accountId;
        TotalInstallments = totalInstallments;
        TotalAmount = totalAmount;
        FirstPaymentDate = firstPaymentDate;
        Status = status;
        InstallmentItems = installmentItems;
        UpdatedAt = updatedAt;
        CreatedAt = createdAt;
    }

    public static CreateInstallmentServiceOutput Factory(IdValueObject id, IdValueObject accountId, int totalInstallments, decimal totalAmount, 
        DateTime firstPaymentDate, string status, CreateInstallmentServiceOutputInstallmentItem[] installmentItems, DateTime? updatedAt, DateTime createdAt)
        => new(id, accountId, totalInstallments, totalAmount, firstPaymentDate, status, installmentItems, updatedAt, createdAt);
}

public sealed class CreateInstallmentServiceOutputInstallmentItem
{
    public IdValueObject Id { get; }
    public int Number { get; }
    public decimal Amount { get; }
    public DateTime DueDate { get; }
    public string Status { get; }
    public DateTime? PaidAt { get; }
    public DateTime? UpdatedAt { get; }
    public DateTime CreatedAt { get; }

    public CreateInstallmentServiceOutputInstallmentItem(IdValueObject id, int number, decimal amount, DateTime dueDate, string status, DateTime? paidAt, 
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
