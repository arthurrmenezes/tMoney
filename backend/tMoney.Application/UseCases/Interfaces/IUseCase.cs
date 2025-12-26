namespace tMoney.Application.UseCases.Interfaces;

public interface IUseCase<TInput, TOutput>
{
    public Task<TOutput> ExecuteUseCaseAsync(TInput input, CancellationToken cancellationToken);
}
