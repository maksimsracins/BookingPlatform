using MediatR;

namespace BookingPlatform.Application.Features.Business.GetBusinesses;

public sealed record GetBusinessesQuery()
    : IRequest<IReadOnlyList<BusinessDto>>;