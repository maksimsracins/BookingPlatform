using BookingPlatform.Application.Common.Abstractions.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingPlatform.Application.Features.Business.GetBusinesses;

public sealed class GetBusinessesHandler
    : IRequestHandler<GetBusinessesQuery, IReadOnlyList<BusinessDto>>
{
    private readonly IBookingDbContext _context;

    public GetBusinessesHandler(
        IBookingDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<BusinessDto>> Handle(
        GetBusinessesQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Businesses
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .Select(x => new BusinessDto(
                x.Id,
                x.Name))
            .ToListAsync(cancellationToken);
    }
}