using tMoney.Domain.BoundedContexts.CardContext.ENUMs;

namespace tMoney.Infrastructure.Data.DTOs;

public record CreditCardInvoiceDto(
    string Id,
    string CardId,
    int Month,
    int Year,
    DateTime CloseDay,
    DateTime DueDay,
    decimal TotalAmount,
    decimal LimitTotal,
    decimal AmountPaid,
    InvoiceStatus Status,
    DateTime? UpdatedAt,
    DateTime CreatedAt);
