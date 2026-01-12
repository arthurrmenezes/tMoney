namespace tMoney.Application.Services.InstallmentContext.Outputs;

public sealed class UpdateInstallmentDetailsByIdServiceOutput
{
    public string Id { get; }
    public string AccountId { get; }
    public int TotalInstallments { get; }
    public decimal TotalAmount { get; }
    public DateTime FirstPaymentDate { get; }
    public string Status { get; }
    public UpdateInstallmentDetailsByIdServiceOutputInstallment[] Installments { get; }
    public DateTime UpdatedAt { get; }
    public DateTime CreatedAt { get; }

    private UpdateInstallmentDetailsByIdServiceOutput(string id, string accountId, int totalInstallments, decimal totalAmount, DateTime firstPaymentDate, 
        string status, UpdateInstallmentDetailsByIdServiceOutputInstallment[] installments, DateTime updatedAt, DateTime createdAt)
    {
        Id = id;
        AccountId = accountId;
        TotalInstallments = totalInstallments;
        TotalAmount = totalAmount;
        FirstPaymentDate = firstPaymentDate;
        Status = status;
        Installments = installments;
        UpdatedAt = updatedAt;
        CreatedAt = createdAt;
    }

    public static UpdateInstallmentDetailsByIdServiceOutput Factory(string id, string accountId, int totalInstallments, decimal totalAmount,
        DateTime firstPaymentDate, string status, UpdateInstallmentDetailsByIdServiceOutputInstallment[] installments, DateTime updatedAt, DateTime createdAt)
        => new(id, accountId, totalInstallments, totalAmount, firstPaymentDate, status, installments, updatedAt, createdAt);
}

public sealed class UpdateInstallmentDetailsByIdServiceOutputInstallment
{
    public string Id { get; }
    public int Number { get; }
    public decimal Amount { get; }
    public DateTime DueDate { get; }
    public string Status { get; }
    public DateTime? PaidAt { get; }
    public DateTime? UpdatedAt { get; }
    public DateTime CreatedAt { get; }

    public UpdateInstallmentDetailsByIdServiceOutputInstallment(string id, int number, decimal amount, DateTime dueDate, string status, DateTime? paidAt, 
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
