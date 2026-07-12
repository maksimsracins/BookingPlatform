using BookingPlatform.Application.Common.Abstractions.Authentication;
using BookingPlatform.Application.Common.Abstractions.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingPlatform.Application.Features.Authentication.GetCurrentUser;

public sealed class GetCurrentUserHandler
    : IRequestHandler<GetCurrentUserQuery, GetCurrentUserResponse>
{
    private readonly IBookingDbContext _context;
    private readonly ICurrentUser _currentUser;

    public GetCurrentUserHandler(
        IBookingDbContext context,
        ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<GetCurrentUserResponse> Handle(
        GetCurrentUserQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .AsNoTracking()
            .Include(x => x.Businesses)
            .ThenInclude(x => x.Business)
            .SingleAsync(
                x => x.Id == _currentUser.UserId,
                cancellationToken);

        var businesses = user.Businesses
            .Select(x => new BusinessDto(
                x.Business.Id,
                x.Business.Name,
                x.Role.ToString()))
            .ToList();

        return new GetCurrentUserResponse(
            user.Id,
            user.Email,
            user.EmailConfirmed,
            businesses);
    }
}