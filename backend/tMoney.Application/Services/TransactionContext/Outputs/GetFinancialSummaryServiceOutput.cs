namespace tMoney.Application.Services.TransactionContext.Outputs;

public sealed class GetFinancialSummaryServiceOutput
{
    public string CardId { get; }
    public decimal PeriodIncome { get; }
    public decimal PeriodExpense { get; }
    public decimal Balance { get; }
    public DateTime StartDate { get; }
    public DateTime EndDate { get; }

    private GetFinancialSummaryServiceOutput(string cardId, decimal periodIncome, decimal periodExpense, decimal balance, DateTime startDate, DateTime endDate)
    {
        CardId = cardId;
        PeriodIncome = periodIncome;
        PeriodExpense = periodExpense;
        Balance = balance;
        StartDate = startDate;
        EndDate = endDate;
    }

    public static GetFinancialSummaryServiceOutput Factory(string cardId, decimal periodIncome, decimal periodExpense, decimal balance, DateTime startDate, 
        DateTime endDate)
        => new(cardId, periodIncome, periodExpense, balance, startDate, endDate);
}
