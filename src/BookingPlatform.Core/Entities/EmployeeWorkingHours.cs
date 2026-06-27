namespace BookingPlatform.Core.Entities;

public class EmployeeWorkingHours
{
    private EmployeeWorkingHours()
    {
    }

    public EmployeeWorkingHours(
        Guid id,
        Guid employeeId,
        DayOfWeek dayOfWeek,
        TimeOnly startTime,
        TimeOnly endTime)
    {
        if (endTime <= startTime)
            throw new ArgumentException("End time must be greater than start time.");

        Id = id;
        EmployeeId = employeeId;
        DayOfWeek = dayOfWeek;
        StartTime = startTime;
        EndTime = endTime;
    }

    public Guid Id { get; private set; }

    public Guid EmployeeId { get; private set; }

    public Employee Employee { get; private set; } = null!;

    public DayOfWeek DayOfWeek { get; private set; }

    public TimeOnly StartTime { get; private set; }

    public TimeOnly EndTime { get; private set; }

    public void ChangeWorkingHours(TimeOnly startTime, TimeOnly endTime)
    {
        if (endTime <= startTime)
            throw new ArgumentException("End time must be greater than start time.");

        StartTime = startTime;
        EndTime = endTime;
    }
}