namespace BookingPlatform.Application.Common.Exceptions;

public sealed class ServiceNotFoundException : Exception
{
    public ServiceNotFoundException(Guid id)
        : base($"Service '{id}' not found.")
    {
    }
}