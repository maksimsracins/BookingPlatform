using BookingPlatform.Core.Entities;

namespace BookingPlatform.Core.Interfaces.Repositories;

public interface IBusinessRepository : IRepository<Business>
{
    Task<Business?> GetByTelegramTokenAsync(string token);

}