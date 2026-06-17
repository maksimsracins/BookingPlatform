namespace BookingPlatform.Core.Entities;

public class Business
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string TelegramBotToken { get; set; } = string.Empty;

    public string TimeZone { get; set; } = "Europe/Riga";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}