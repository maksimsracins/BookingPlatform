using BookingPlatform.Application.Common.Abstractions.Authentication;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BookingPlatform.Infrastructure.Authentication;

public sealed class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId
    {
        get
        {
            var value = _httpContextAccessor.HttpContext?.User
                .FindFirstValue(JwtRegisteredClaimNames.Sub);

            var claims = _httpContextAccessor.HttpContext!.User.Claims
                .Select(c => $"{c.Type} = {c.Value}")
                .ToList();

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new UnauthorizedAccessException();
            }

            return Guid.Parse(value);
        }
    }

    public string Email =>
        _httpContextAccessor.HttpContext?.User
            .FindFirstValue(JwtRegisteredClaimNames.Email)
        ?? throw new UnauthorizedAccessException();
}