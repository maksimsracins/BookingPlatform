using BookingPlatform.Application.Features.Booking.GetAvailableSlots;

namespace BookingPlatform.Application.Features.Booking.GetAvailableSlots;
public sealed record GetAvailableSlotsResult(
    IReadOnlyCollection<AvailableSlotDto> Slots);