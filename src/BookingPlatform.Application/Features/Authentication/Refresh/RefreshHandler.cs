using BookingPlatform.Application.Authentication;
using BookingPlatform.Application.Common.Abstractions.Authentication;
using BookingPlatform.Application.Common.Abstractions.Persistance;
using BookingPlatform.Application.Common.Authentication;
using BookingPlatform.Application.Common.Exceptions;
using BookingPlatform.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BookingPlatform.Application.Features.Authentication.Refresh;

public sealed class RefreshHandler
    : IRequestHandler<RefreshCommand, AuthenticationResult>
{
    private readonly IBookingDbContext _context;
    private readonly IJwtProvider _jwtProvider;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly ITokenHasher _tokenHasher;
    private readonly AuthenticationOptions _options;

    public RefreshHandler(
        IBookingDbContext context,
        IJwtProvider jwtProvider,
        IRefreshTokenGenerator refreshTokenGenerator,
        ITokenHasher tokenHasher,
        IOptions<AuthenticationOptions> options)
    {
        _context = context;
        _jwtProvider = jwtProvider;
        _refreshTokenGenerator = refreshTokenGenerator;
        _tokenHasher = tokenHasher;
        _options = options.Value;
    }

    public async Task<AuthenticationResult> Handle(RefreshCommand command, CancellationToken cancellationToken)
    {
        var hash = _tokenHasher.Hash(command.RefreshToken);

        var refreshToken = await _context.RefreshTokens
            .Include(x => x.User)
            .SingleOrDefaultAsync(
                x => x.TokenHash == hash,
                cancellationToken);

        if (!refreshToken.IsActive(DateTime.UtcNow)){
            throw new InvalidCredentialException();
        }

        var jwt = _jwtProvider.Generate(
            new JwtDescriptor(
                refreshToken.User.Id,
                refreshToken.User.Email));

        refreshToken.Revoke();

        var newRefresh = _refreshTokenGenerator.Generate();

        _context.RefreshTokens.Add(
            RefreshToken.Create(
                refreshToken.UserId,
                _tokenHasher.Hash(newRefresh),
                DateTime.UtcNow.AddDays(
                    _options.RefreshTokenExpirationDays)));

        await _context.SaveChangesAsync(cancellationToken);

        return new AuthenticationResult(
            jwt,
            newRefresh);
    }
}