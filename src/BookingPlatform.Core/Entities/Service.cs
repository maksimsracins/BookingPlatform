namespace BookingPlatform.Core.Entities;

public class Service
{

    private Service()
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
        IsActive = true;
    }

    public Guid Id { get; private set; }

    public Guid BusinessId { get; private set; }
    public Business Business { get; private set; } = null!;

    public string Name { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public decimal Price { get; private set; }

    public TimeSpan Duration { get; private set; }

    public bool IsActive { get; private set; } = true;

    public void ChangePrice(decimal price)
    {
        if (price < 0)
            throw new ArgumentException("Price cannot be negative.", nameof(price)); 
        
        Price = price;
    }
    public void ChangeDuration(TimeSpan duration)
    {
        if (duration <= TimeSpan.Zero)
            throw new ArgumentException("Duration must be greater than zero.", nameof(duration));

        Duration = duration;
    }
    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty.", nameof(name));

        Name = name;
    }
    public void Deactivate()
    {
        IsActive = false;
    }
}