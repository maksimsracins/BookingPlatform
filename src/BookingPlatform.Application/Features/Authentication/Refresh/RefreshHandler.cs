using BookingPlatform.Application.Common.Abstractions.Authentication;
using BookingPlatform.Application.Common.Abstractions.Persistance;
using BookingPlatform.Application.Common.Exceptions;
using BookingPlatform.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingPlatform.Application.Features.Authentication.Refresh;

public sealed class RefreshHandler : IRequestHandler<RefreshCommand, RefreshResponse>
{
    private readonly IBookingDbContext _context;
    private readonly IJwtProvider _jwtProvider;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly ITokenHasher _tokenHasher;

    public RefreshHandler(
        IBookingDbContext context,
        IJwtProvider jwtProvider,
        IRefreshTokenGenerator refreshTokenGenerator,
        ITokenHasher tokenHasher)
    {
        _context = context;
        _jwtProvider = jwtProvider;
        _refreshTokenGenerator = refreshTokenGenerator;
        _tokenHasher = tokenHasher;
    }

    public async Task<RefreshResponse> Handle(
        RefreshCommand command,
        CancellationToken cancellationToken)
    {
        var tokenHash = _tokenHasher.Hash(command.RefreshToken);

        var storedToken = await _context.RefreshTokens
            .Include(x => x.User)
            .SingleOrDefaultAsync(
                x => x.TokenHash == tokenHash,
                cancellationToken);

        if (storedToken is null)
        {
            throw new InvalidRefreshTokenException();
        }

        if (storedToken.RevokedAt is not null)
        {
            // A revoked token being presented again means it was replayed or leaked, so end every session for this user.
            await RevokeActiveTokensAsync(storedToken.UserId, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            throw new InvalidRefreshTokenException();
        }

        if (storedToken.ExpiresAt <= DateTime.UtcNow || !storedToken.User.IsActive)
        {
            throw new InvalidRefreshTokenException();
        }

        storedToken.Revoke();

        var newRefreshToken = _refreshTokenGenerator.Generate();

        _context.RefreshTokens.Add(
            new RefreshToken(
                Guid.NewGuid(),
                storedToken.UserId,
                _tokenHasher.Hash(newRefreshToken),
                DateTime.UtcNow.Add(RefreshToken.DefaultLifetime),
                deviceName: null,
                ipAddress: null));

        var jwt = _jwtProvider.Generate(
            new JwtDescriptor(
                storedToken.User.Id,
                storedToken.User.Email));

        await _context.SaveChangesAsync(cancellationToken);

        return new RefreshResponse(
            jwt.AccessToken,
            newRefreshToken,
            jwt.ExpiresAt);
    }

    private async Task RevokeActiveTokensAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var activeTokens = await _context.RefreshTokens
            .Where(x => x.UserId == userId && x.RevokedAt == null)
            .ToListAsync(cancellationToken);

        foreach (var token in activeTokens)
        {
            token.Revoke();
        }
    }
}
