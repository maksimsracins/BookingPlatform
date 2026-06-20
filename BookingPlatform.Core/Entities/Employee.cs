namespace BookingPlatform.Core.Entities;

public class Employee
{
    public Guid Id { get; set; }

    public Guid BusinessId { get; set; }
    public ICollection<Appointment> Appointments { get; set; } = [];

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}