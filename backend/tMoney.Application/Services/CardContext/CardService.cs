using tMoney.Application.Services.CardContext.Inputs;
using tMoney.Application.Services.CardContext.Interfaces;
using tMoney.Application.Services.CardContext.Outputs;
using tMoney.Domain.BoundedContexts.CardContext.Entities;
using tMoney.Domain.BoundedContexts.CardContext.ENUMs;
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
        Card card;
        CreateCardServiceOutputCreditCard? creditCardOutput = null;

        switch (input.CardType)
        {
            case CardType.DebitCard:
                if (input.CreditCard is not null)
                    throw new ArgumentException("Cartão de débito não deve ter limite ou datas.");

                card = new DebitCard(
                    accountId: accountId,
                    name: input.Name);
                break;

            case CardType.CreditCard:
                if (input.CreditCard is null)
                    throw new ArgumentException("Dados do cartão de crédito são obrigatórios.");

                var creditCard = new CreditCard(
                    accountId: accountId,
                    name: input.Name,
                    limit: input.CreditCard.Limit,
                    closeDay: input.CreditCard.CloseDay,
                    dueDay: input.CreditCard.DueDay);

                card = creditCard;

                creditCardOutput = CreateCardServiceOutputCreditCard.Factory(
                    limit: creditCard.Limit,
                    closeDay: creditCard.CloseDay,
                    dueDay: creditCard.DueDay);
                break;

            default:
                throw new ArgumentException("Tipo de cartão inválido.");
        }

        await _cardRepository.AddAsync(card, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var output = CreateCardServiceOutput.Factory(
            id: card.Id.ToString(),
            accountId: card.AccountId.ToString(),
            name: card.Name,
            type: card.Type.ToString(),
            creditCard: creditCardOutput,
            updatedAt: card.UpdatedAt,
            createdAt: card.CreatedAt);

        return output;
    }

    public async Task<GetCardByIdServiceOutput> GetCardByIdServiceAsync(
        IdValueObject cardId,
        IdValueObject accountId,
        CancellationToken cancellationToken)
    {
        var card = await _cardRepository.GetByIdAsync(cardId.Id, accountId.Id, false, cancellationToken);
        if (card is null)
            throw new KeyNotFoundException("Cartão não foi encontrado.");

        GetCardByIdServiceOutputCreditCard? creditCardOutput = null;
        if (card is CreditCard creditCardEntity)
            creditCardOutput = GetCardByIdServiceOutputCreditCard.Factory(
                limit: creditCardEntity.Limit,
                closeDay: creditCardEntity.CloseDay,
                dueDay: creditCardEntity.DueDay);

        var output = GetCardByIdServiceOutput.Factory(
            id: card.Id.ToString(),
            accountId: card.AccountId.ToString(),
            name: card.Name,
            type: card.Type.ToString(),
            creditCard: creditCardOutput,
            updatedAt: card.UpdatedAt,
            createdAt: card.CreatedAt);

        return output;
    }

    public async Task<GetAllCardsByAccountIdServiceOutput> GetAllCardsByAccountIdServiceAsync(
        IdValueObject accountId,
        GetAllCardsByAccountIdServiceInput input,
        CancellationToken cancellationToken)
    {
        var cards = await _cardRepository.GetAllByAccountId(accountId.Id, input.PageNumber, input.PageSize, input.CardType, cancellationToken);

        var totalCards = await _cardRepository.GetTotalCardsNumberAsync(accountId.Id, cancellationToken);

        var cardsOutput = cards.Select(c =>
        {
            if (c is CreditCard creditCard)
                return GetAllCardsByAccountIdServiceOutputCards.Factory(
                    id: c.Id.ToString(),
                    accountId: c.AccountId.ToString(),
                    name: c.Name,
                    type: c.Type.ToString(),
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
                    type: c.Type.ToString(),
                    creditCard: null,
                    updatedAt: c.UpdatedAt,
                    createdAt: c.CreatedAt);
        }).ToArray();

        var totalPages = (int)Math.Ceiling((double)totalCards / input.PageSize);

        var output = GetAllCardsByAccountIdServiceOutput.Factory(
            totalCards: totalCards,
            pageNumber: input.PageNumber,
            pageSize: input.PageSize,
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

            if (card is CreditCard c)
                creditCardOutput = UpdateCardByIdServiceOutputCreditCard.Factory(
                    limit: c.Limit,
                    closeDay: c.CloseDay,
                    dueDay: c.DueDay);

            var output = UpdateCardByIdServiceOutput.Factory(
                id: card.Id.ToString(),
                accountId: card.AccountId.ToString(),
                name: card.Name,
                type: card.Type.ToString(),
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

    public async Task DeleteCardByIdServiceAsync(
        IdValueObject cardId,
        IdValueObject accountId,
        CancellationToken cancellationToken)
    {
        var card = await _cardRepository.GetByIdAsync(cardId.Id, accountId.Id, true, cancellationToken);
        if (card is null)
            throw new ArgumentException("Cartão não foi encontrado.");

        _cardRepository.Delete(card);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<CreateInvoiceByCardIdServiceOutput> CreateInvoiceByCardIdServiceAsync(
        IdValueObject cardId, 
        IdValueObject accountId,
        DateTime firstPaymentDate,
        int totalInstallments,
        CancellationToken cancellationToken)
    {
        var card = await _cardRepository.GetByIdAsync(cardId.Id, accountId.Id, false, cancellationToken);
        if (card is null)
            throw new KeyNotFoundException("Cartão não foi encontrado.");

        if (card is not CreditCard)
            throw new ArgumentException("Não é possível criar uma fatura com o cartão de débito.");

        var invoices = new List<CreditCardInvoice>();
        var creditCard = (CreditCard)card;

        for (int i = 0; i < totalInstallments; i++)
        {
            var referenceDate = firstPaymentDate.AddMonths(i);

            var tempInvoice = creditCard.GenerateInvoice(referenceDate);

            var existingInvoice = await _creditCardInvoiceRepository.GetByDateAsync(
                card.Id.Id,
                tempInvoice.Year,
                tempInvoice.Month,
                cancellationToken);

            if (existingInvoice is null)
            {
                await _creditCardInvoiceRepository.AddAsync(tempInvoice, cancellationToken);
                invoices.Add(tempInvoice);
            }
            else
                invoices.Add(existingInvoice);
        }

        var output = CreateInvoiceByCardIdServiceOutput.Factory(
            invoices: invoices.Select(i => CreateInvoiceByCardIdServiceOutputInvoices.Factory(
                invoiceId: i.Id.ToString(),
                cardId: i.CreditCardId.ToString(),
                accountId: card.AccountId.ToString(),
                cardName: card.Name,
                month: i.Month,
                year: i.Year,
                closeDay: i.CloseDay,
                dueDay: i.DueDay,
                amountPaid: i.AmountPaid,
                limit: i.LimitTotal,
                status: i.Status.ToString(),
                updatedAt: i.UpdatedAt,
                createdAt: i.CreatedAt)).ToArray());

        return output;
    }
}
