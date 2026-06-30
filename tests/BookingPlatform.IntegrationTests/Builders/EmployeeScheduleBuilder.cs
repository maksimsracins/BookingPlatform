using BookingPlatform.Core.Entities;

namespace BookingPlatform.IntegrationTests.Builders;

public sealed class EmployeeWorkingHoursBuilder
{
    private Guid _id = Guid.NewGuid();
    private Guid _employeeId = Guid.NewGuid();

    private DayOfWeek _dayOfWeek = DayOfWeek.Monday;
    private TimeOnly _startTime = new(9, 0);
    private TimeOnly _endTime = new(18, 0);

    public EmployeeWorkingHoursBuilder ForEmployee(Guid employeeId)
    {
        _employeeId = employeeId;
        return this;
    }

    public EmployeeWorkingHoursBuilder ForDay(DayOfWeek dayOfWeek)
    {
        _dayOfWeek = dayOfWeek;
        return this;
    }

    public EmployeeWorkingHoursBuilder From(TimeOnly startTime)
    {
        _startTime = startTime;
        return this;
    }

    public EmployeeWorkingHoursBuilder To(TimeOnly endTime)
    {
        _endTime = endTime;
        return this;
    }

    public EmployeeWorkingHours Build()
    {
        return new EmployeeWorkingHours(
            _id,
            _employeeId,
            _dayOfWeek,
            _startTime,
            _endTime);
    }
}