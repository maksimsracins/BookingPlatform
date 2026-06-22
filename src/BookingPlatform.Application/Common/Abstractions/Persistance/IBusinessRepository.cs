using BookingPlatform.Core.Entities;
using BookingPlatform.Application.Interfaces.Repositories;

namespace BookingPlatform.Application.Interfaces.Repositories;

public interface IBusinessRepository : IRepository<Business>
{
    Task<Business?> GetByTelegramTokenAsync(string token);

}