using BookingPlatform.Core.Enums;

namespace BookingPlatform.Core.Entities;

public class Appointment
{
    public Guid Id { get; set; }

    public Guid BusinessId { get; set; }

    public Guid ClientId { get; set; }

    public Guid EmployeeId { get; set; }

    public Guid ServiceId { get; set; }

    public DateTime StartAt { get; set; }

    public DateTime EndAt { get; set; }

    public AppointmentStatus Status { get; set; }
}