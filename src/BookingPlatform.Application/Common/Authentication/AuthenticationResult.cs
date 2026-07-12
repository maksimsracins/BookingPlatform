public sealed record AuthenticationResult(
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAt);