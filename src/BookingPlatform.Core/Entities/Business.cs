namespace BookingPlatform.Core.Entities;

public class Business
{
    private Business()
    {
    }

    private Business(string name, string phone, string address, string timeZone, TimeSpan slotInterval)
    {   
        Id = Guid.NewGuid();
        Name = name;
        Phone = phone;
        Address = address;
        TimeZone = timeZone;
        IsActive = true;
        SlotInterval = slotInterval;
    }

    public Guid Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string Phone { get; private set; } = string.Empty;

    public string Address { get; private set; } = string.Empty;

    public string TimeZone { get; private set; } = "Europe/Riga";
    public TimeSpan SlotInterval { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public ICollection<Service> Services { get; private set; } = [];

    public ICollection<Employee> Employees { get; private set; } = [];
    public ICollection<Client> Clients { get; } = [];

    public ICollection<Appointment> Appointments { get; } = [];
    private readonly List<BusinessUser> _users = [];

    public IReadOnlyCollection<BusinessUser> Users => _users;

    public void ChangePhone(string newPhone)
    {
        Phone = newPhone;
        Update();
    }
    public void ChangeAddress(string newAddress)
    {
        Address = newAddress;
        Update();
    }

    public void Activate()
    {
        IsActive = true;
        Update();
    }

    public void Deactivate()
    {
        IsActive = false;
        Update();
    }

    public void Update()
    {
        UpdatedAt = DateTime.UtcNow;
    }

    public static Business Create(string name)
    {
        return new Business(
            name,
            string.Empty,
            string.Empty,
            "Europe/Riga",
            TimeSpan.FromMinutes(15));
    }
}