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
}
