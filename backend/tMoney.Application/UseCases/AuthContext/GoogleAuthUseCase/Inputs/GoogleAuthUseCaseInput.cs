namespace tMoney.Application.UseCases.AuthContext.GoogleAuthUseCase.Inputs;

public sealed class GoogleAuthUseCaseInput
{
    public string GoogleToken { get; }

    private GoogleAuthUseCaseInput(string googleToken)
    {
        GoogleToken = googleToken;
    }

    public static GoogleAuthUseCaseInput Factory(string googleToken)
        => new(googleToken);
}
