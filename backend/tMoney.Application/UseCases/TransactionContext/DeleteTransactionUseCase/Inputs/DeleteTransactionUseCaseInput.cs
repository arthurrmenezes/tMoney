using tMoney.Domain.ValueObjects;

namespace tMoney.Application.UseCases.TransactionContext.DeleteTransactionUseCase.Inputs;

public sealed class DeleteTransactionUseCaseInput
{
    public IdValueObject TransactionId { get; }
    public IdValueObject AccountId { get; }

    private DeleteTransactionUseCaseInput(IdValueObject transactionId, IdValueObject accountId)
    {
        TransactionId = transactionId;
        AccountId = accountId;
    }

    public static DeleteTransactionUseCaseInput Factory(IdValueObject transactionId, IdValueObject accountId)
        => new(transactionId, accountId);
}
