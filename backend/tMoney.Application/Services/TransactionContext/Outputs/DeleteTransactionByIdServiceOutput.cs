namespace tMoney.Application.Services.TransactionContext.Outputs;

public sealed class DeleteTransactionByIdServiceOutput
{
    public string? Installment { get; }

    private DeleteTransactionByIdServiceOutput(string? installment)
    {
        Installment = installment;
    }

    public static DeleteTransactionByIdServiceOutput Factory(string? installment)
        => new(installment);
}
