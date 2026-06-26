using BookingPlatform.Core.Entities;

namespace BookingPlatform.IntegrationTests.Builders;

public sealed class ServiceBuilder
{
    private Guid _id = Guid.NewGuid();
    private Guid _businessId = Guid.NewGuid();

    private string _name = "Haircut";
    private string _description = "Classic haircut";
    private decimal _price = 20m;
    private TimeSpan _duration = TimeSpan.FromMinutes(30);

    public ServiceBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public ServiceBuilder ForBusiness(Guid businessId)
    {
        _businessId = businessId;
        return this;
    }

    public ServiceBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public ServiceBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public ServiceBuilder WithPrice(decimal price)
    {
        _price = price;
        return this;
    }

    public ServiceBuilder WithDuration(TimeSpan duration)
    {
        _duration = duration;
        return this;
    }

    public Service Build()
    {
        return new Service(
            _id,
            _businessId,
            _name,
            _description,
            _price,
            _duration);
    }
}