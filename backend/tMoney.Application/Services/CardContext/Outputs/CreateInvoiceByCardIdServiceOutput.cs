namespace tMoney.Application.Services.CardContext.Outputs;

public sealed class CreateInvoiceByCardIdServiceOutput
{
    public CreateInvoiceByCardIdServiceOutputInvoices[] Invoices { get; }

    private CreateInvoiceByCardIdServiceOutput(CreateInvoiceByCardIdServiceOutputInvoices[] invoices)
    {
        Invoices = invoices;
    }

    public static CreateInvoiceByCardIdServiceOutput Factory(CreateInvoiceByCardIdServiceOutputInvoices[] invoices)
        => new(invoices);
}

public sealed class CreateInvoiceByCardIdServiceOutputInvoices
{
    public string InvoiceId { get; }
    public string CardId { get; }
    public string AccountId { get; }
    public string CardName { get; }
    public int Month { get; }
    public int Year { get; }
    public DateTime CloseDay { get; }
    public DateTime DueDay { get; }
    public decimal AmountPaid { get; }
    public decimal Limit { get; }
    public string Status { get; }
    public DateTime? UpdatedAt { get; }
    public DateTime CreatedAt { get; }

    public CreateInvoiceByCardIdServiceOutputInvoices(string invoiceId, string cardId, string accountId, string cardName, int month, int year, 
        DateTime closeDay, DateTime dueDay, decimal amountPaid, decimal limit, string status, DateTime? updatedAt, DateTime createdAt)
    {
        InvoiceId = invoiceId;
        CardId = cardId;
        AccountId = accountId;
        CardName = cardName;
        Month = month;
        Year = year;
        CloseDay = closeDay;
        DueDay = dueDay;
        AmountPaid = amountPaid;
        Limit = limit;
        Status = status;
        UpdatedAt = updatedAt;
        CreatedAt = createdAt;
    }

    public static CreateInvoiceByCardIdServiceOutputInvoices Factory(string invoiceId, string cardId, string accountId, string cardName, int month, int year,
        DateTime closeDay, DateTime dueDay, decimal amountPaid, decimal limit, string status, DateTime? updatedAt, DateTime createdAt)
        => new(invoiceId, cardId, accountId, cardName, month, year, closeDay, dueDay, amountPaid, limit, status, updatedAt, createdAt);
}
