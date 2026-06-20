namespace BookingPlatform.Application.Common.Abstractions.Persistance;
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}   
