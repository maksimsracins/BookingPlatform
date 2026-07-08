using System.Net.Mail;

namespace BookingPlatform.Core.Entities;

public sealed class User
{
    private readonly List<BusinessUser> _businesses = [];
    private readonly List<RefreshToken> _refreshTokens = [];

    private User()
    {
    }

    public User(
        Guid id,
        string email,
        string passwordHash)
    {
        Id = id;
        Email = NormalizeEmail(email);
        PasswordHash = passwordHash;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }

    public string Email { get; private set; }

    public string PasswordHash { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public IReadOnlyCollection<BusinessUser> Businesses => _businesses;

    public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens;

    public void ChangePassword(string passwordHash)
    {
        PasswordHash = passwordHash;
    }

    private static string NormalizeEmail(string email)
    {
        var normalized = email.Trim().ToLowerInvariant();

        _ = new MailAddress(normalized);

        return normalized;
    }
}