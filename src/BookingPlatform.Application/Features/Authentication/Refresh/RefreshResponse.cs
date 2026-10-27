namespace BookingPlatform.Application.Features.Authentication.Refresh;

public sealed record RefreshResponse(string AccessToken, string RefreshToken, DateTime ExpiresAt);
