using BookingPlatform.Application.Interfaces.Repositories;
using BookingPlatform.Infrastructure.Persistence.Context;

namespace BookingPlatform.Infrastructure.Persistence;

public class UnitOfWork(BookingDbContext context) : IUnitOfWork
{
    private readonly BookingDbContext _context = context;

    public async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}