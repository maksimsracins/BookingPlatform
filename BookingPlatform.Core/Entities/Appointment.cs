using BookingPlatform.Core.Enums;

namespace BookingPlatform.Core.Entities;

public class Appointment
{
    // For EF Core
    public Appointment() { }
    public Appointment(Guid businessId, Guid clientId, Guid employeeId, Guid serviceId, DateTime startAt, DateTime endAt)
    {
        Id = Guid.NewGuid();
        BusinessId = businessId;
        ClientId = clientId;
        EmployeeId = employeeId;
        ServiceId = serviceId;
        StartAt = startAt;
        EndAt = endAt;
        Status = AppointmentStatus.Pending;
    }
    public Guid Id { get; private set; }

    public Guid BusinessId { get; private set; }

    public Guid ClientId { get; private set; }

    public Guid EmployeeId { get; private set; }

    public Guid ServiceId { get; private set; }

    public DateTime StartAt { get; private set; }

    public DateTime EndAt { get; private set; }

    public AppointmentStatus Status { get; private set; }

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
}