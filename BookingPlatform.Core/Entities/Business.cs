namespace BookingPlatform.Core.Entities;

public class Business
{
    private Business()
    {
    }

    public Business(string name, string phone, string address, string telegramBotToken, string timeZone)
    {
        Id = Guid.NewGuid();
        Name = name;
        Phone = phone;
        Address = address;
        TimeZone = timeZone;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public Guid Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string Phone { get; private set; } = string.Empty;

    public string Address { get; private set; } = string.Empty;

    public string TimeZone { get; private set; } = "Europe/Riga";
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public ICollection<Service> Services { get; private set; } = [];

    public ICollection<Employee> Employees { get; private set; } = [];
    public ICollection<Client> Clients { get; } = [];

    public ICollection<Appointment> Appointments { get; } = [];

    public void ChangePhone(string newPhone)
    {
        Phone = newPhone;
    }
    public void ChangeAddress(string newAddress)
    {
        Address = newAddress;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}