using BookingPlatform.Core.Entities;

public interface IClientRepository
{
    Task<Client?> GetByIdAsync(Guid id);

    Task<Client?> GetByTelegramIdAsync(long telegramUserId);

    Task AddAsync(Client client);

    Task UpdateAsync(Client client);
}