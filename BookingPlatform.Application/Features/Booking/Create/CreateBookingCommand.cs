namespace BookingPlatform.Application.Features.Booking.Create;

public sealed class CreateBookingCommand
{
    public Guid BusinessId { get; init; }

    public Guid ServiceId { get; init; }

    public Guid EmployeeId { get; init; }

    public long TelegramUserId { get; init; }

    public DateTime StartAt { get; init; }
}