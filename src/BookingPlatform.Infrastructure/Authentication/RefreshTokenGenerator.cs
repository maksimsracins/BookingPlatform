namespace BookingPlatform.Infrastructure.Authentication;

using System.Security.Cryptography;
using BookingPlatform.Application.Common.Abstractions.Authentication;

public sealed class RefreshTokenGenerator : IRefreshTokenGenerator
{
    public string Generate()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }
}