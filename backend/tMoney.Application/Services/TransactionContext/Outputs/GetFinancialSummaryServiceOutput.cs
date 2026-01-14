namespace tMoney.Application.Services.TransactionContext.Outputs;

public sealed class GetFinancialSummaryServiceOutput
{
    public decimal PeriodIncome { get; }
    public decimal PeriodExpense { get; }
    public decimal Balance { get; }
    public DateTime StartDate { get; }
    public DateTime EndDate { get; }

    private GetFinancialSummaryServiceOutput(decimal periodIncome, decimal periodExpense, decimal balance, DateTime startDate, DateTime endDate)
    {
        PeriodIncome = periodIncome;
        PeriodExpense = periodExpense;
        Balance = balance;
        StartDate = startDate;
        EndDate = endDate;
    }

    public static GetFinancialSummaryServiceOutput Factory(decimal periodIncome, decimal periodExpense, decimal balance, DateTime startDate, DateTime endDate)
        => new(periodIncome, periodExpense, balance, startDate, endDate);
}
