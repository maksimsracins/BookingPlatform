using BookingPlatform.Core.Entities;

namespace BookingPlatform.Application.Common.Scheduling;

public sealed class AvailableSlotsCalculator
{
    public static IReadOnlyCollection<AvailableSlot> Calculate(
    TimeOnly workStart,
    TimeOnly workEnd,
    TimeSpan duration,
    IReadOnlyCollection<Appointment> appointments,
    DateOnly date)
{
    var result = new List<AvailableSlot>();

    var current = date.ToDateTime(workStart);

    var workEndDateTime = date.ToDateTime(workEnd);

    while (current.Add(duration) <= workEndDateTime)
    {
        var slotEnd = current.Add(duration);

        var isBusy = appointments.Any(x =>
            x.StartAt < slotEnd &&
            x.EndAt > current);

        if (!isBusy)
        {
            result.Add(new AvailableSlot(
                current,
                slotEnd));
        }

        current = current.Add(duration);
    }

    return result;
}
}