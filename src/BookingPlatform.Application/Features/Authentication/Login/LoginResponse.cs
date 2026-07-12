namespace BookingPlatform.Application.Features.Authentication.Login;

public sealed record LoginResponse(string AccessToken, DateTime ExpiresAt);