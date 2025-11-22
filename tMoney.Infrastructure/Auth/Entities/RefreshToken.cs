using tMoney.Domain.ValueObjects;

namespace tMoney.Infrastructure.Auth.Entities;

public class RefreshToken
{
    public IdValueObject Id { get; private set; }
    public string Token { get; private set; }
    public string UserId { get; private set; }
    public string? ReplacedByToken { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public DateTime? RevokedAt { get; private set; }

    protected RefreshToken() { }

    public RefreshToken(string token, string userId, DateTime expiresAt)
    {
        Id = IdValueObject.New();
        Token = token;
        UserId = userId;
        ReplacedByToken = null;
        CreatedAt = DateTime.UtcNow;
        ExpiresAt = expiresAt;
        RevokedAt = null;
    }

    public bool IsExpired()
    {
        return DateTime.UtcNow >= ExpiresAt;
    }

    public bool IsRevoked()
    {
        return RevokedAt != null;
    }

    public bool IsActive()
    {
        return !IsExpired() && !IsRevoked();
    }

    public void Revoke(string replacementToken)
    {
        RevokedAt = DateTime.UtcNow;
        ReplacedByToken = replacementToken;
    }
}
