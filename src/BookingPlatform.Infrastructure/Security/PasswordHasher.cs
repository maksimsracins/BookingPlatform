using BookingPlatform.Application.Common.Abstractions.Security;
using BookingPlatform.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace BookingPlatform.Infrastructure.Security;

public sealed class PasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<User> _passwordHasher = new();

    public string Hash(string password)
    {
        return _passwordHasher.HashPassword(null!, password);
    }

    public bool Verify(
        string password,
        string passwordHash)
    {
        var result = _passwordHasher.VerifyHashedPassword(
            null!,
            passwordHash,
            password);

        return result != PasswordVerificationResult.Failed;
    }
}