using BookingPlatform.Application.Common.Abstractions.Authentication;
using BookingPlatform.Application.Common.Abstractions.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;

public sealed class LogoutHandler : IRequestHandler<LogoutCommand>
{
    private readonly IBookingDbContext _context;
    private readonly ITokenHasher _tokenHasher;

    public LogoutHandler(IBookingDbContext context, ITokenHasher tokenHasher)
    {
        _context = context;
        _tokenHasher = tokenHasher;
    }

    public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
            return;

        var hash = _tokenHasher.Hash(request.RefreshToken);

        var refreshToken = await _context.RefreshTokens
            .SingleOrDefaultAsync(
                x => x.TokenHash == hash,
                cancellationToken);

        if (refreshToken is null)
            return;

        refreshToken.Revoke(DateTime.UtcNow);

        await _context.SaveChangesAsync(cancellationToken);
    }
}