using tMoney.Domain.ValueObjects;

namespace tMoney.Application.UseCases.TransactionContext.GetAllTransactionsByInvoiceIdUseCase.Inputs;

public sealed class GetAllTransactionsByInvoiceIdUseCaseInput
{
    public IdValueObject AccountId { get; }
    public IdValueObject CardId { get; }
    public IdValueObject InvoiceId { get; }
    public int PageNumber { get; }
    public int PageSize { get; }

    private GetAllTransactionsByInvoiceIdUseCaseInput(IdValueObject accountId, IdValueObject cardId, IdValueObject invoiceId, int pageNumber, int pageSize)
    {
        AccountId = accountId;
        CardId = cardId;
        InvoiceId = invoiceId;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }

    public static GetAllTransactionsByInvoiceIdUseCaseInput Factory(IdValueObject accountId, IdValueObject cardId, IdValueObject invoiceId, int pageNumber,
        int pageSize)
        => new(accountId, cardId, invoiceId, pageNumber, pageSize);
}
