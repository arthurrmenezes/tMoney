using Microsoft.EntityFrameworkCore;
using tMoney.Infrastructure.Auth.Entities;
using tMoney.Infrastructure.Data.Repositories.Base;
using tMoney.Infrastructure.Data.Repositories.Interfaces;

namespace tMoney.Infrastructure.Data.Repositories;

public sealed class RefreshTokenRepository : BaseRepository<RefreshToken>, IRefreshTokenRepository
{
    public RefreshTokenRepository(DataContext dataContext) : base(dataContext) { }

    public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken)
    {
        return await _dataContext.RefreshTokens
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Token == token, cancellationToken);
    }

    public async Task RevokeAllByUserIdAsync(string userId, CancellationToken cancellationToken)
    {
        await _dataContext.RefreshTokens.Where(u => 
                u.UserId == userId && 
                u.RevokedAt == null &&
                DateTime.UtcNow < u.ExpiresAt &&
                u.ReplacedByToken == null)
            .ExecuteUpdateAsync(calls => calls.SetProperty(r => r.RevokedAt, DateTime.UtcNow), cancellationToken);
    }
}
