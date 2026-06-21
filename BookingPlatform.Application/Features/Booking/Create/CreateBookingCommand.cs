namespace BookingPlatform.Application.Features.Booking.Create;

public sealed record CreateBookingCommand
(
    Guid BusinessId,
    Guid ServiceId,
    Guid EmployeeId,
    Guid ClientId, 
    DateTime StartAt);