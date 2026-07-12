public sealed record JwtToken(
    string AccessToken,
    DateTime ExpiresAt);