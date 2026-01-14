namespace tMoney.Application.UseCases.Interfaces;

public interface IUseCase
{
    public Task ExecuteUseCaseAsync(CancellationToken cancellationToken);
}

public interface IUseCase<TInput>
{
    public Task ExecuteUseCaseAsync(TInput input, CancellationToken cancellationToken);
}

public interface IUseCase<TInput, TOutput>
{
    public Task<TOutput> ExecuteUseCaseAsync(TInput input, CancellationToken cancellationToken);
}
