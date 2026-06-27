using BookingPlatform.Core.Entities;

namespace BookingPlatform.Application.Common.Scheduling;

public sealed class AvailableSlotsCalculator
{
    public IReadOnlyCollection<AvailableSlot> Calculate(
    TimeOnly workStart,
    TimeOnly workEnd,
    TimeSpan serviceDuration,
    TimeSpan slotInterval,
    IReadOnlyCollection<Appointment> appointments,
    DateOnly date)
{
    var slots = new List<AvailableSlot>();

    var current = date.ToDateTime(workStart);

    var endOfWorkingDay = date.ToDateTime(workEnd);

    while (current.Add(serviceDuration) <= endOfWorkingDay)
    {
        var slotEnd = current.Add(serviceDuration);

        var isBusy = appointments.Any(x =>
            x.StartAt < slotEnd &&
            x.EndAt > current);

        if (!isBusy)
        {
            slots.Add(new AvailableSlot(
                current,
                slotEnd));
        }

        current = current.Add(slotInterval);
    }

    return slots;
}
}