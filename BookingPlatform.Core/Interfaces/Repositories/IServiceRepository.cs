using BookingPlatform.Core.Entities;

namespace BookingPlatform.Core.Interfaces.Repositories;

public interface IServiceRepository : IRepository<Service>
{
    Task<IReadOnlyList<Service>> GetByBusinessAsync(Guid businessId);

}