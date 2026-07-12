namespace BookingPlatform.Application.Features.Authentication.GetCurrentUser;

public sealed record GetCurrentUserResponse(
    Guid Id,
    string Email,
    bool EmailConfirmed,
    IReadOnlyCollection<BusinessDto> Businesses);

public sealed record BusinessDto(
    Guid Id,
    string Name,
    string Role);