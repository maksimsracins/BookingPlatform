using BookingPlatform.Core.Enums;

namespace BookingPlatform.Core.Entities;

public class Appointment
{
    // For EF Core
    private Appointment() { }
    public Appointment(Guid businessId, Guid clientId, Guid employeeId, Guid serviceId, decimal price, DateTime startAt, DateTime endAt)
    {
        Id = Guid.NewGuid();
        BusinessId = businessId;
        ClientId = clientId;
        EmployeeId = employeeId;
        ServiceId = serviceId;
        Price = price;
        StartAt = startAt;
        EndAt = endAt;
        Status = AppointmentStatus.Pending;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;
    }
    public Guid Id { get; private set; }

    public Guid BusinessId { get; private set; }
    public Business Business { get; private set; } = null!;

    public Guid ClientId { get; private set; }
    public Client Client { get; private set; } = null!;

    public Guid EmployeeId { get; private set; }
    public Employee Employee { get; private set; } = null!;

    public Guid ServiceId { get; private set; }
    public Service Service { get; private set; } = null!;
    public decimal Price { get; private set; }
    public TimeSpan Duration { get; private set; }

    public DateTime StartAt { get; private set; }

    public DateTime EndAt { get; private set; }

    public AppointmentStatus Status { get; private set; }
    public string Comment { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public void Confirm()
    {
        if (Status != AppointmentStatus.Pending)
            throw new InvalidOperationException("Only pending appointments can be confirmed.");

        Status = AppointmentStatus.Confirmed;
    }

    public void Cancel()
    {
        if (Status == AppointmentStatus.Cancelled)
            throw new InvalidOperationException("Appointment is already cancelled.");

        Status = AppointmentStatus.Cancelled;
    }

    public void Complete()
    {
        if (Status != AppointmentStatus.Confirmed)
            throw new InvalidOperationException();

        Status = AppointmentStatus.Completed;
    }
    public void Reschedule(DateTime startAt, DateTime endAt)
    {
        if (Status == AppointmentStatus.Completed)
            throw new InvalidOperationException();

        StartAt = startAt;
        EndAt = endAt;
    }
}