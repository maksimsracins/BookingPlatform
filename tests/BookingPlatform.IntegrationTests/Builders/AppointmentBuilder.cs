using BookingPlatform.Core.Entities;

public sealed class AppointmentBuilder
{
    private Guid _id = Guid.NewGuid();

    private Guid _businessId;

    private Guid _clientId;

    private Guid _employeeId;

    private Guid _serviceId;

    private decimal _price = 20;

    private TimeSpan _duration = TimeSpan.FromMinutes(30);

    private DateTime _startAt = DateTime.UtcNow.AddHours(2);

    public AppointmentBuilder ForBusiness(Guid id)
    {
        _businessId = id;
        return this;
    }

    public AppointmentBuilder ForClient(Guid id)
    {
        _clientId = id;
        return this;
    }

    public AppointmentBuilder ForEmployee(Guid id)
    {
        _employeeId = id;
        return this;
    }

    public AppointmentBuilder ForService(Guid id)
    {
        _serviceId = id;
        return this;
    }

    public AppointmentBuilder StartingAt(DateTime start)
    {
        _startAt = start;
        return this;
    }

    public AppointmentBuilder WithPrice(decimal price)
    {
        _price = price;
        return this;
    }

    public AppointmentBuilder WithDuration(TimeSpan duration)
    {
        _duration = duration;
        return this;
    }

    public Appointment Build()
    {
        return new Appointment(
            _id,
            _businessId,
            _clientId,
            _employeeId,
            _serviceId,
            _price,
            _duration,
            _startAt,
            _startAt.Add(_duration));
    }
}