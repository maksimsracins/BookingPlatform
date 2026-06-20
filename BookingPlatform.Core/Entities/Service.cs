namespace BookingPlatform.Core.Entities;

public class Service
{

    public Service()
    {
        
    }

    public Service(Guid businessId, string name, string description, decimal price, TimeSpan duration)
    {
        Id = Guid.NewGuid();
        BusinessId = businessId;
        Name = name;
        Description = description;
        Price = price;
        Duration = duration;
    }

    public Guid Id { get; private set; }

    public Guid BusinessId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public decimal Price { get; private set; }

    public TimeSpan Duration { get; private set; }

    public bool IsActive { get; private set; } = true;
}