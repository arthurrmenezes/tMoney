using tMoney.Infrastructure.Services.TokenService.Interfaces;

namespace tMoney.WebApi.WorkerServices.TokenService;

public class DeleteInvalidRefreshTokensJob : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public DeleteInvalidRefreshTokensJob(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var now = DateTime.UtcNow;
        var nextRun = now.Date.AddHours(7);

        if (now > nextRun)
            nextRun = nextRun.AddDays(1);

        var initialDelay = nextRun - now;

        await Task.Delay(initialDelay, stoppingToken);

        using var timer = new PeriodicTimer(TimeSpan.FromHours(24));

        do
        {
            using var service = _serviceScopeFactory.CreateScope();
            var tokenService = service.ServiceProvider.GetRequiredService<ITokenService>();

            await tokenService.DeleteInvalidRefreshTokensAsync(stoppingToken);
        }
        while (await timer.WaitForNextTickAsync(stoppingToken));
    }
}
