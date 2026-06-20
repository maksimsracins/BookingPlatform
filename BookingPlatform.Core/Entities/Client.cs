namespace BookingPlatform.Core.Entities;

public class Client
{
    private Client() { }
    public Client(Guid businessId, string fullName, string phone, string telegramUserName, string notes, long telegramUserId)
    {
        Id = Guid.NewGuid();
        BusinessId = businessId;
        FullName = fullName;
        Phone = phone;
        TelegramUserName = telegramUserName;
        Notes = notes;
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