using BookingPlatform.Core.Entities;

namespace BookingPlatform.Application.Common.Abstractions.Authentication;

public interface IJwtProvider
{
    JwtToken Generate(JwtDescriptor descriptor);
}