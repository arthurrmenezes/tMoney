using tMoney.Domain.BoundedContexts.CardContext.ENUMs;

namespace tMoney.Application.Services.CardContext.Inputs;

public sealed class GetAllCardsByAccountIdServiceInput
{
    public int PageNumber { get; }
    public int PageSize { get; }
    public CardType? CardType { get; }

    private GetAllCardsByAccountIdServiceInput(int pageNumber, int pageSize, CardType? cardType)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        CardType = cardType;
    }

    public static GetAllCardsByAccountIdServiceInput Factory(int pageNumber, int pageSize, CardType? cardType)
        => new(pageNumber, pageSize, cardType);
}
