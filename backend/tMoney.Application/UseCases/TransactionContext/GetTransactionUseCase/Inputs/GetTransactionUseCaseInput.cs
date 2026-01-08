using tMoney.Domain.ValueObjects;

namespace tMoney.Application.UseCases.TransactionContext.GetTransactionUseCase.Inputs;

public sealed class GetTransactionUseCaseInput
{
    public IdValueObject TransactionId { get; }
    public IdValueObject AccountId { get; }

    private GetTransactionUseCaseInput(IdValueObject transactionId, IdValueObject accountId)
    {
        TransactionId = transactionId;
        AccountId = accountId;
    }

    public static GetTransactionUseCaseInput Factory(IdValueObject transactionId, IdValueObject accountId)
        => new(transactionId, accountId);
}
