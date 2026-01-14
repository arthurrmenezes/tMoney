using tMoney.Application.UseCases.TransactionContext.UpdateOverdueTransactionsUseCase;

namespace tMoney.WebApi.WorkerServices.TransactionContext;

public class TransactionOverdueJob : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public TransactionOverdueJob(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var now = DateTime.UtcNow;
        var nextRun = now.Date.AddMinutes(1);

        if (now > nextRun)
            nextRun = nextRun.AddDays(1);

        var initialDelay = nextRun - now;

        await Task.Delay(initialDelay, stoppingToken);

        using var timer = new PeriodicTimer(TimeSpan.FromHours(24));

        do
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var useCase = scope.ServiceProvider.GetRequiredService<UpdateOverdueTransactionsUseCase>();

            await useCase.ExecuteUseCaseAsync(stoppingToken);
        }
        while (await timer.WaitForNextTickAsync(stoppingToken));
    }
}
