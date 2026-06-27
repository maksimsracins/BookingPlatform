namespace BookingPlatform.Application.Features.Booking.GetAvailableSlots;
public sealed class GetAvailableSlotsQuery
{
    public Guid BusinessId { get; init; }

    public Guid EmployeeId { get; init; }

    public Guid ServiceId { get; init; }

    public DateOnly Date { get; init; }
}

public sealed record AvailableSlotDto(
    DateTime StartAt,
    DateTime EndAt);