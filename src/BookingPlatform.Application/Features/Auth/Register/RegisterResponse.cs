namespace BookingPlatform.Application.Features.Auth.Register;

public sealed record RegisterResponse(
    Guid UserId,
    Guid BusinessId);