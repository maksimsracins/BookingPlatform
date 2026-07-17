namespace BookingPlatform.Core.Entities;

public sealed class RefreshToken
{
    private RefreshToken()
    {
    }

    private RefreshToken(
        Guid id,
        Guid userId,
        string tokenHash,
        DateTime expiresAt,
        string? deviceName,
        string? ipAddress)
    {
        Id = id;
        UserId = userId;
        TokenHash = tokenHash;
        ExpiresAt = expiresAt;
        DeviceName = deviceName;
        IpAddress = ipAddress;
        CreatedAt = DateTime.UtcNow;
    }

    public static RefreshToken Create(Guid userId, string tokenHash, DateTime expiresAt)
    {
        return new RefreshToken(
            Guid.NewGuid(),
            userId,
            tokenHash,
            expiresAt,
            null,
            null);
    }

    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }

    public string TokenHash { get; private set; } = default!;

    public DateTime ExpiresAt { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? RevokedAt { get; private set; }

    public string? DeviceName { get; private set; }

    public string? IpAddress { get; private set; }

    public User User { get; private set; } = default!;

    public bool IsExpired(DateTime utcNow)
    => ExpiresAt <= utcNow;

    public bool IsRevoked()
        => RevokedAt is not null;

    public bool IsActive(DateTime utcNow)
        => !IsRevoked() && !IsExpired(utcNow);

    public void Revoke()
    {
        RevokedAt = DateTime.UtcNow;
    }
}