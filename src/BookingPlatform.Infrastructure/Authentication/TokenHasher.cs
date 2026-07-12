namespace BookingPlatform.Infrastructure.Authentication;

using System.Security.Cryptography;
using System.Text;
using BookingPlatform.Application.Common.Abstractions.Authentication;

public sealed class TokenHasher : ITokenHasher
{
    public string Hash(string token)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));

        return Convert.ToHexString(bytes);
    }
}