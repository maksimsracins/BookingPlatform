namespace BookingPlatform.Application.Common.Abstractions.Authentication;

public interface ICurrentUser
{
    Guid UserId { get; }

    string Email { get; }
}