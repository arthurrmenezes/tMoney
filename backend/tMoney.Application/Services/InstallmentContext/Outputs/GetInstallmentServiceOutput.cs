namespace tMoney.Application.Services.InstallmentContext.Outputs;

public sealed class GetInstallmentServiceOutput
{
    public string Id { get; }
    public string AccountId { get; }
    public int TotalInstallments { get; }
    public decimal TotalAmount { get; }
    public DateTime FirstPaymentDate { get; }
    public string Status { get; }
    public GetInstallmentServiceOutputInstallmentItem[] Installments { get; }
    public DateTime? UpdatedAt { get; }
    public DateTime CreatedAt { get; }

    private GetInstallmentServiceOutput(string id, string accountId, int totalInstallments, decimal totalAmount, DateTime firstPaymentDate, string status, 
        GetInstallmentServiceOutputInstallmentItem[] installments, DateTime? updatedAt, DateTime createdAt)
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

    public static GetInstallmentServiceOutput Factory(string id, string accountId, int totalInstallments, decimal totalAmount, DateTime firstPaymentDate,
        string status, GetInstallmentServiceOutputInstallmentItem[] installments, DateTime? updatedAt, DateTime createdAt)
        => new(id, accountId, totalInstallments, totalAmount, firstPaymentDate, status, installments, updatedAt, createdAt);
}

public sealed class GetInstallmentServiceOutputInstallmentItem
{
    public string Id { get; }
    public string InstallmentId { get; }
    public int Number { get; }
    public decimal Amount { get; }
    public DateTime DueDate { get; }
    public string Status { get; }
    public DateTime? PaidAt { get; }
    public DateTime? UpdatedAt { get; }
    public DateTime CreatedAt { get; }

    public GetInstallmentServiceOutputInstallmentItem(string id, string installmentId, int number, decimal amount, DateTime dueDate, string status, 
        DateTime? paidAt, DateTime? updatedAt, DateTime createdAt)
    {
        Id = id;
        InstallmentId = installmentId;
        Number = number;
        Amount = amount;
        DueDate = dueDate;
        Status = status;
        PaidAt = paidAt;
        UpdatedAt = updatedAt;
        CreatedAt = createdAt;
    }
}
