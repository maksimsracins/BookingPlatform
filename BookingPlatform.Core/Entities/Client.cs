namespace BookingPlatform.Core.Entities;

public class Client
{
    public Guid Id { get; set; }

    public Guid BusinessId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public long TelegramUserId { get; set; }
}