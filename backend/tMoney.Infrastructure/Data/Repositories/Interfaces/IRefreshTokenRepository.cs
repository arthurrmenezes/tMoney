using tMoney.Infrastructure.Auth.Entities;
using tMoney.Infrastructure.Data.Repositories.Base.Interfaces;

namespace tMoney.Infrastructure.Data.Repositories.Interfaces;

public interface IRefreshTokenRepository : IBaseRepository<RefreshToken>
{
    public Task<RefreshToken?> GetByTokenAsync(string accesstoken, CancellationToken cancellationToken);
    public Task RevokeAllByUserIdAsync(string userId, CancellationToken cancellationToken);
    public Task DeleteInvalidRefreshTokensAsync(CancellationToken cancellationToken);
}
