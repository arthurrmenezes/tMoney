namespace tMoney.Application.Services.InstallmentContext.Outputs;

public sealed class GetAllInstallmentItemsByInvoiceIdServiceOutput
{
    public string Id { get; }
    public string InstallmentId { get; }
    public string InvoiceId { get; }
    public string Title { get; }
    public int TotalInstallments { get; }
    public string CategoryId { get; }
    public int Number { get; }
    public decimal Amount { get; }
    public DateTime DueDate { get; }
    public string Status { get; }
    public DateTime? PaidAt { get; }
    public DateTime? UpdatedAt { get; }
    public DateTime CreatedAt { get; }

    private GetAllInstallmentItemsByInvoiceIdServiceOutput(string id, string installmentId, string invoiceId, string title, int totalInstallments, 
        string categoryId, int number, decimal amount, DateTime dueDate, string status, DateTime? paidAt, DateTime? updatedAt, DateTime createdAt)
    {
        Id = id;
        InstallmentId = installmentId;
        InvoiceId = invoiceId;
        Title = title;
        TotalInstallments = totalInstallments;
        CategoryId = categoryId;
        Number = number;
        Amount = amount;
        DueDate = dueDate;
        Status = status;
        PaidAt = paidAt;
        UpdatedAt = updatedAt;
        CreatedAt = createdAt;
    }

    public static GetAllInstallmentItemsByInvoiceIdServiceOutput Factory(string id, string installmentId, string invoiceId, string title, int totalInstallments,
        string categoryId, int number, decimal amount, DateTime dueDate, string status, DateTime? paidAt, DateTime? updatedAt, DateTime createdAt)
        => new(id, installmentId, invoiceId, title, totalInstallments, categoryId, number, amount, dueDate, status, paidAt, updatedAt, createdAt);
}
