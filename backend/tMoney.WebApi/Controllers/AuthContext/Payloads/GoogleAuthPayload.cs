namespace tMoney.WebApi.Controllers.AuthContext.Payloads;

public sealed class GoogleAuthPayload
{
    public string GoogleToken { get; init; }

    public GoogleAuthPayload(string googleToken)
    {
        GoogleToken = googleToken;
    }
}
