namespace BookingPlatform.Application.Common.Authentication;

public sealed record AuthenticationResult(
    JwtToken AccessToken,
    string RefreshToken);