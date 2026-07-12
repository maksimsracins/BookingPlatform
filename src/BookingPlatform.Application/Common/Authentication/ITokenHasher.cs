namespace BookingPlatform.Application.Common.Abstractions.Authentication;

public interface ITokenHasher
{
    string Hash(string value);
}