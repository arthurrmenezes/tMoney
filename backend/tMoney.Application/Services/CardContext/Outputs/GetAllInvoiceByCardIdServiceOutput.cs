using tMoney.Domain.BoundedContexts.CardContext.ENUMs;

namespace tMoney.Application.Services.CardContext.Outputs;

public sealed class GetAllInvoiceByCardIdServiceOutput
{
    public int TotalInvoices { get; }
    public int PageNumber { get; }
    public int PageSize { get; }
    public int TotalPages { get; }
    public GetAllInvoiceByCardIdServiceOutputInvoice[] Invoices { get; }

    private GetAllInvoiceByCardIdServiceOutput(int totalInvoices, int pageNumber, int pageSize, int totalPages, 
        GetAllInvoiceByCardIdServiceOutputInvoice[] invoices)
    {
        TotalInvoices = totalInvoices;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalPages = totalPages;
        Invoices = invoices;
    }

    public static GetAllInvoiceByCardIdServiceOutput Factory(int totalInvoices, int pageNumber, int pageSize, int totalPages,
        GetAllInvoiceByCardIdServiceOutputInvoice[] invoices)
        => new(totalInvoices, pageNumber, pageSize, totalPages, invoices);
}

public sealed class GetAllInvoiceByCardIdServiceOutputInvoice
{
    public string Id { get; }
    public string CardId { get; }
    public int Month { get; }
    public int Year { get; }
    public DateTime CloseDay { get; }
    public DateTime DueDay { get; }
    public decimal TotalAmount { get; }
    public decimal LimitTotal { get; }
    public decimal AmountPaid { get; }
    public InvoiceStatus Status { get; }
    public DateTime? UpdatedAt { get; }
    public DateTime CreatedAt { get; }

    private GetAllInvoiceByCardIdServiceOutputInvoice(string id, string cardId, int month, int year, DateTime closeDay, DateTime dueDay, decimal totalAmount,
        decimal limitTotal, decimal amountPaid, InvoiceStatus status, DateTime? updatedAt, DateTime createdAt)
    {
        Id = id;
        CardId = cardId;
        Month = month;
        Year = year;
        CloseDay = closeDay;
        DueDay = dueDay;
        TotalAmount = totalAmount;
        LimitTotal = limitTotal;
        AmountPaid = amountPaid;
        Status = status;
        UpdatedAt = updatedAt;
        CreatedAt = createdAt;
    }

    public static GetAllInvoiceByCardIdServiceOutputInvoice Factory(string id, string cardId, int month, int year, DateTime closeDay, DateTime dueDay,
        decimal totalAmount, decimal limitTotal, decimal amountPaid, InvoiceStatus status, DateTime? updatedAt, DateTime createdAt)
        => new(id, cardId, month, year, closeDay, dueDay, totalAmount, limitTotal, amountPaid, status, updatedAt, createdAt);
}
