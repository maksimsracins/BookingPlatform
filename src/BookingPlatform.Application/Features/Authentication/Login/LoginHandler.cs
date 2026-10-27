using BookingPlatform.Application.Common.Abstractions.Authentication;
using BookingPlatform.Application.Common.Abstractions.Persistance;
using BookingPlatform.Application.Common.Abstractions.Security;
using BookingPlatform.Application.Common.Exceptions;
using BookingPlatform.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingPlatform.Application.Features.Authentication.Login;

public sealed class LoginHandler : IRequestHandler<LoginCommand,LoginResponse>
{
    private readonly IBookingDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly ITokenHasher _tokenHasher;

    public LoginHandler(
        IBookingDbContext context,
        IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider,
        IRefreshTokenGenerator refreshTokenGenerator,
        ITokenHasher tokenHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
        _refreshTokenGenerator = refreshTokenGenerator;
        _tokenHasher = tokenHasher;
    }

    public async Task<LoginResponse> Handle(
        LoginCommand command,
        CancellationToken cancellationToken)
    {
        var email = command.Email.Trim().ToLowerInvariant();

        var user = await _context.Users
            .SingleOrDefaultAsync(
                x => x.Email == email,
                cancellationToken);

        if (user is null)
        {
            throw new InvalidCredentialException();
        }

        var validPassword = _passwordHasher.Verify(
            command.Password,
            user.PasswordHash);

        if (!validPassword)
        {
            throw new InvalidCredentialException();
        }

        var jwt = _jwtProvider.Generate(
            new JwtDescriptor(
                user.Id,
                user.Email));

        var refreshToken = _refreshTokenGenerator.Generate();

        _context.RefreshTokens.Add(
            new RefreshToken(
                Guid.NewGuid(),
                user.Id,
                _tokenHasher.Hash(refreshToken),
                DateTime.UtcNow.Add(RefreshToken.DefaultLifetime),
                deviceName: null,
                ipAddress: null));

        await _context.SaveChangesAsync(cancellationToken);

        return new LoginResponse(
            jwt.AccessToken,
            refreshToken,
            jwt.ExpiresAt);
    }
}
