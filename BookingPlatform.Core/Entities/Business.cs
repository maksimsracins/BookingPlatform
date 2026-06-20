namespace BookingPlatform.Core.Entities;

public class Business
{
    public Business()
    {
    }

    public Business(string name, string phone, string address, string telegramBotToken, string timeZone)
    {
        Id = Guid.NewGuid();
        Name = name;
        Phone = phone;
        Address = address;
        TelegramBotToken = telegramBotToken;
        TimeZone = timeZone;
        IsActive = true;
    }

    public Guid Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string Phone { get; private set; } = string.Empty;

    public string Address { get; private set; } = string.Empty;

    public string TelegramBotToken { get; private set; } = string.Empty;

    public string TimeZone { get; private set; } = "Europe/Riga";
    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public ICollection<Service> Services { get; private set; } = [];

    public ICollection<Employee> Employees { get; private set; } = [];

    public void ChangePhone(string newPhone)
    {
        Phone = newPhone;
    }

    public void Activate()
    {
        IsActive = true;
    }
}