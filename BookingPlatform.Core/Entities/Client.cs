namespace BookingPlatform.Core.Entities;

public class Client
{
    private Client() { }
    public Client(Guid id, Guid businessId, string fullName, string telegramUserName, string phone, long telegramUserId)
    {
        Id = id;
        BusinessId = businessId;
        FullName = fullName;
        TelegramUserName = telegramUserName;
        Phone = phone;
        TelegramUserId = telegramUserId;
    }

    public Guid Id { get; private set; }
    public Guid BusinessId { get; private set; }
    public Business Business { get; private set; } = null!;
    public string FullName { get; private set; } = string.Empty;
    public string TelegramUserName { get; private set; } = string.Empty;
    public string Notes { get; private set; } = string.Empty;
    public string Phone { get; private set; } = string.Empty;

    public long TelegramUserId { get; private set; }

    public ICollection<Appointment> Appointments { get; private set; } = [];

    public void ChangePhone(string phone)
    {
        Phone = phone;
    }

}