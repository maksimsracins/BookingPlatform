using BookingPlatform.Core.Entities;
using BookingPlatform.Application.Interfaces.Repositories;

public interface IClientRepository : IRepository<Client>
{

    Task<Client?> GetByTelegramIdAsync(long telegramUserId);

}