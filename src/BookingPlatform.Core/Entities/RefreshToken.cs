namespace BookingPlatform.Core.Entities;

public sealed class RefreshToken
{
    public static readonly TimeSpan DefaultLifetime = TimeSpan.FromDays(7);

    private RefreshToken()

    {
    }

    public RefreshToken(
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

    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }

    public string TokenHash { get; private set; } = default!;

    public DateTime ExpiresAt { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? RevokedAt { get; private set; }

    public string? DeviceName { get; private set; }

    public string? IpAddress { get; private set; }

    public User User { get; private set; } = default!;

    public void Revoke()
    {
        RevokedAt = DateTime.UtcNow;
    }
}