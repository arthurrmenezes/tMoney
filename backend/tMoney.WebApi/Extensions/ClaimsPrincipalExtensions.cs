using System.Security.Claims;

namespace tMoney.WebApi.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetAccountId(this ClaimsPrincipal user)
    {
        var accountIdString = user?.FindFirst("accountId")?.Value;
        if (string.IsNullOrEmpty(accountIdString))
            throw new UnauthorizedAccessException("Token inválido ou sem identificador de conta.");

        if (!Guid.TryParse(accountIdString, out var accountIdGuid))
            throw new ArgumentException("ID da conta inválido no token");

        return accountIdGuid;
    }
}
