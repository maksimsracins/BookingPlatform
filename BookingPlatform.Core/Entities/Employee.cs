namespace BookingPlatform.Core.Entities;

public class Employee
{
    private Employee()
    {
        
    }

    public Employee(Guid businessId, string fullName, string phone, string color)
    {
        Id = Guid.NewGuid();
        BusinessId = businessId;
        FullName = fullName;
        Phone = phone;
        Color = color;
        IsActive = true;
    }
    public Guid Id { get; private set; }

    public Guid BusinessId { get; private set; }
    public Business Business { get; private set; } = null!;

    public string FullName { get; private set; } = string.Empty;


    public string Phone { get; private set; } = string.Empty;

    public bool IsActive { get; private set; } = true;
    public string Color { get; private set; } = string.Empty;
    public ICollection<Appointment> Appointments { get; private set; } = [];

    public void ChangePhone(string phone)
    {
        Phone = phone;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
    
}