namespace BookingPlatform.Application.Common.Abstractions.Authentication;

public interface IRefreshTokenGenerator
{
    string Generate();
}