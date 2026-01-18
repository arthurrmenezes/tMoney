using tMoney.Application.Services.CardContext.Inputs;
using tMoney.Application.Services.CardContext.Interfaces;
using tMoney.Application.Services.CardContext.Outputs;
using tMoney.Domain.BoundedContexts.CardContext.Entities;
using tMoney.Domain.ValueObjects;
using tMoney.Infrastructure.Data.Repositories.Interfaces;
using tMoney.Infrastructure.Data.UnitOfWork.Interfaces;

namespace tMoney.Application.Services.CardContext;

public class CardService : ICardService
{
    private readonly ICardRepository _cardRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CardService(ICardRepository cardRepository, IUnitOfWork unitOfWork)
    {
        _cardRepository = cardRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateCardServiceOutput> CreateCardServiceAsync(
        IdValueObject accountId, 
        CreateCardServiceInput input, 
        CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            Card card;
            CreateCardServiceOutputCreditCard? createCardServiceOutputCreditCard = null;

            if (input.CreditCard is not null)
            {
                var creditCard = new CreditCard(
                    accountId: accountId,
                    name: input.Name,
                    limit: input.CreditCard.Limit,
                    closeDay: input.CreditCard.CloseDay,
                    dueDay: input.CreditCard.DueDay);

                await _cardRepository.AddAsync(creditCard, cancellationToken);

                card = creditCard;

                createCardServiceOutputCreditCard = CreateCardServiceOutputCreditCard.Factory(
                    limit: creditCard.Limit,
                    closeDay: creditCard.CloseDay,
                    dueDay: creditCard.DueDay);
            }
            else
            {
                card = new DebitCard(
                    accountId: accountId,
                    name: input.Name);

                await _cardRepository.AddAsync(card, cancellationToken);
            }

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            var output = CreateCardServiceOutput.Factory(
                id: card.Id.ToString(),
                accountId: card.AccountId.ToString(),
                name: card.Name,
                creditCard: createCardServiceOutputCreditCard,
                updatedAt: card.UpdatedAt,
                createdAt: card.CreatedAt);

            return output;
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}
