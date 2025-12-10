namespace tMoney.Application.Services.TransactionContext.Outputs;

public sealed class GetFinancialSummaryServiceOutput
{
    public decimal TotalIncome { get; }
    public decimal TotalExpense { get; }
    public decimal Balance { get; }
    public DateTime StartDate { get; }
    public DateTime EndDate { get; }

    private GetFinancialSummaryServiceOutput(decimal totalIncome, decimal totalExpense, decimal balance, DateTime startDate, DateTime endDate)
    {
        TotalIncome = totalIncome;
        TotalExpense = totalExpense;
        Balance = balance;
        StartDate = startDate;
        EndDate = endDate;
    }

    public static GetFinancialSummaryServiceOutput Factory(decimal totalIncome, decimal totalExpense, decimal balance, DateTime startDate, DateTime endDate)
        => new(totalIncome, totalExpense, balance, startDate, endDate);
}
