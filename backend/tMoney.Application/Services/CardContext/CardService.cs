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
    private readonly ICreditCardInvoiceRepository _creditCardInvoiceRepository;

    public CardService(ICardRepository cardRepository, IUnitOfWork unitOfWork, ICreditCardInvoiceRepository creditCardInvoiceRepository)
    {
        _cardRepository = cardRepository;
        _unitOfWork = unitOfWork;
        _creditCardInvoiceRepository = creditCardInvoiceRepository;
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

    public async Task<GetCardByIdServiceOutput> GetCardByIdServiceAsync(
        IdValueObject cardId, 
        IdValueObject accountId, 
        CancellationToken cancellationToken)
    {
        var card = await _cardRepository.GetByIdAsync(cardId.Id, accountId.Id, false, cancellationToken);
        if (card is null)
            throw new ArgumentException("Cartão não foi encontrado.");

        var cardType = "Debit";
        
        GetCardByIdServiceOutputCreditCard? creditCardOutput = null;
        if (card is CreditCard creditCardEntity)
        {
            creditCardOutput = GetCardByIdServiceOutputCreditCard.Factory(
                limit: creditCardEntity.Limit,
                closeDay: creditCardEntity.CloseDay,
                dueDay: creditCardEntity.DueDay);

            cardType = "Credit";
        }

        var output = GetCardByIdServiceOutput.Factory(
            id: card.Id.ToString(),
            accountId: card.AccountId.ToString(),
            name: card.Name,
            type: cardType,
            creditCard: creditCardOutput,
            updatedAt: card.UpdatedAt,
            createdAt: card.CreatedAt);

        return output;
    }

    public async Task<GetAllCardsByAccountIdServiceOutput> GetAllCardsByAccountIdServiceAsync(
        IdValueObject accountId, 
        int pageNumber, 
        int pageSize, 
        CancellationToken cancellationToken)
    {
        var cards = await _cardRepository.GetAllByAccountId(accountId.Id, pageNumber, pageSize, cancellationToken);

        var totalCards = await _cardRepository.GetTotalCardsNumberAsync(accountId.Id, cancellationToken);

        var cardsOutput = cards.Select(c =>
        {
            if (c is CreditCard creditCard)
                return GetAllCardsByAccountIdServiceOutputCards.Factory(
                    id: c.Id.ToString(),
                    accountId: c.AccountId.ToString(),
                    name: c.Name,
                    type: "CreditCard",
                    creditCard: GetAllCardsByAccountIdServiceOutputCreditCard.Factory(
                        limit: creditCard.Limit,
                        closeDay: creditCard.CloseDay,
                        dueDay: creditCard.DueDay),
                    updatedAt: c.UpdatedAt,
                    createdAt: c.CreatedAt);
            else
                return GetAllCardsByAccountIdServiceOutputCards.Factory(
                    id: c.Id.ToString(),
                    accountId: c.AccountId.ToString(),
                    name: c.Name,
                    type: "DebitCard",
                    creditCard: null,
                    updatedAt: c.UpdatedAt,
                    createdAt: c.CreatedAt);
        }).ToArray();

        var totalPages = (int)Math.Ceiling((double)totalCards / pageSize);

        var output = GetAllCardsByAccountIdServiceOutput.Factory(
            totalCards: totalCards,
            pageNumber: pageNumber,
            pageSize: pageSize,
            totalPages: totalPages,
            cards: cardsOutput);

        return output;
    }

    public async Task<UpdateCardByIdServiceOutput> UpdateCardByIdServiceAsync(
        IdValueObject cardId,
        IdValueObject accountId, 
        UpdateCardByIdServiceInput input, 
        CancellationToken cancellationToken)
    {
        var card = await _cardRepository.GetByIdAsync(cardId.Id, accountId.Id, true, cancellationToken);
        if (card is null)
            throw new ArgumentException("Cartão não foi encontrado.");

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            card.UpdateCardDetails(input.Name);

            if (card is CreditCard creditCard)
            { 
                if (input.CreditCard is not null)
                {
                    creditCard.UpdateCreditCardDetails(
                        limit: input.CreditCard.Limit,
                        closeDay: input.CreditCard.CloseDay,
                        dueDay: input.CreditCard.DueDay);

                    var currentInvoice = await _creditCardInvoiceRepository.GetOpenInvoiceAsync(card.Id.Id, cancellationToken);

                    if (currentInvoice is not null && input.CreditCard.Limit.HasValue)
                    {
                        currentInvoice.UpdateLimit(input.CreditCard.Limit.Value);

                        _creditCardInvoiceRepository.Update(currentInvoice);
                    }                        
                }
            }
            else
                if (input.CreditCard is not null)
                    throw new ArgumentException("Não é possível alterar o tipo do cartão.");

            _cardRepository.Update(card);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            UpdateCardByIdServiceOutputCreditCard? creditCardOutput = null;
            var cardType = "DebitCard";

            if (card is CreditCard c)
            {
                creditCardOutput = UpdateCardByIdServiceOutputCreditCard.Factory(
                    limit: c.Limit,
                    closeDay: c.CloseDay,
                    dueDay: c.DueDay);

                cardType = "CreditCard";
            }

            var output = UpdateCardByIdServiceOutput.Factory(
                id: card.Id.ToString(),
                accountId: card.AccountId.ToString(),
                name: card.Name,
                type: cardType,
                creditCard: creditCardOutput,
                updatedAt: card.UpdatedAt,
                createdAt: card.CreatedAt);

            return output;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}
