using BookingPlatform.Application.Authentication;
using BookingPlatform.Application.Common.Abstractions.Authentication;
using BookingPlatform.Application.Common.Abstractions.Persistance;
using BookingPlatform.Application.Common.Abstractions.Security;
using BookingPlatform.Application.Common.Authentication;
using BookingPlatform.Application.Common.Exceptions;
using BookingPlatform.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BookingPlatform.Application.Features.Authentication.Login;

public sealed class LoginHandler : IRequestHandler<LoginCommand,AuthenticationResult>
{
    private readonly IBookingDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly ITokenHasher _tokenHasher;
    private readonly IOptions<AuthenticationOptions> _authenticationOptions;

    public LoginHandler(
        IBookingDbContext context,
        IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider,
        IRefreshTokenGenerator refreshTokenGenerator,
        IOptions<AuthenticationOptions> authenticationOptions,
        ITokenHasher tokenHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
        _refreshTokenGenerator = refreshTokenGenerator;
        _tokenHasher = tokenHasher;
        _authenticationOptions = authenticationOptions;
    }

    public async Task<AuthenticationResult> Handle(
        LoginCommand command,
        CancellationToken cancellationToken)
    {
        var email = command.Email.Trim().ToLowerInvariant();

        var user = await _context.Users
            .SingleOrDefaultAsync(
                x => x.Email == email,
                cancellationToken);

        if (user is null)
            throw new InvalidCredentialException();

        if (!_passwordHasher.Verify(
                command.Password,
                user.PasswordHash))
        {
            throw new InvalidCredentialException();
        }

        var jwt = _jwtProvider.Generate(
            new JwtDescriptor(
                user.Id,
                user.Email));

        var refreshToken = _refreshTokenGenerator.Generate();

        var refreshTokenHash = _tokenHasher.Hash(refreshToken);

        var refresh = RefreshToken.Create(
            user.Id,
            refreshTokenHash,
            jwt.ExpiresAt.AddDays(_authenticationOptions.Value.RefreshTokenExpirationDays));

        _context.RefreshTokens.Add(refresh);

        await _context.SaveChangesAsync(cancellationToken);

        return new AuthenticationResult(
            jwt,
            refreshToken);
    }
}