using BookingPlatform.Core.Enums;

namespace BookingPlatform.Core.Entities;

public sealed class BusinessUser
{
    private BusinessUser()
    {
    }

    public BusinessUser(
        Guid businessId,
        Guid userId,
        BusinessRole role)
    {
        BusinessId = businessId;
        UserId = userId;
        Role = role;
    }

    public Guid BusinessId { get; private set; }

    public Guid UserId { get; private set; }

    public BusinessRole Role { get; private set; }

    public Business Business { get; private set; } = default!;

    public User User { get; private set; } = default!;
}