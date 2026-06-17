namespace BookingPlatform.Core.Entities;

public class Service
{
    public Guid Id { get; set; }

    public Guid BusinessId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public TimeSpan Duration { get; set; }

    public bool IsActive { get; set; } = true;
}