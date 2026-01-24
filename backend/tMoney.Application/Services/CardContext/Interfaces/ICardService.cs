using tMoney.Application.Services.CardContext.Inputs;
using tMoney.Application.Services.CardContext.Outputs;
using tMoney.Domain.ValueObjects;

namespace tMoney.Application.Services.CardContext.Interfaces;

public interface ICardService
{
    public Task<CreateCardServiceOutput> CreateCardServiceAsync(
        IdValueObject accountId,
        CreateCardServiceInput input,
        CancellationToken cancellationToken);

    public Task<GetCardByIdServiceOutput> GetCardByIdServiceAsync(
        IdValueObject cardId,
        IdValueObject accountId,
        CancellationToken cancellationToken);

    public Task<GetAllCardsByAccountIdServiceOutput> GetAllCardsByAccountIdServiceAsync(
        IdValueObject accountId,
        GetAllCardsByAccountIdServiceInput input,
        CancellationToken cancellationToken);

    public Task<UpdateCardByIdServiceOutput> UpdateCardByIdServiceAsync(
        IdValueObject cardId,
        IdValueObject accountId,
        UpdateCardByIdServiceInput input,
        CancellationToken cancellationToken);

    public Task DeleteCardByIdServiceAsync(
        IdValueObject cardId,
        IdValueObject accountId,
        CancellationToken cancellationToken);

    public Task<CreateInvoiceByCardIdServiceOutput> CreateInvoiceByCardIdServiceAsync(
        IdValueObject cardId,
        IdValueObject accountId,
        DateTime firstPaymentDate,
        int totalInstallments,
        CancellationToken cancellationToken);

    public Task<GetAllInvoiceByCardIdServiceOutput> GetAllInvoiceByCardIdServiceAsync(
        IdValueObject cardId,
        IdValueObject accountId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken);
}
