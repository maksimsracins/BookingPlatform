namespace BookingPlatform.Application.Common.Exceptions;

public sealed class ClientNotFoundException : Exception
{
    public ClientNotFoundException(Guid id)
        : base($"Email '{id}' not found.")
    {
    }
}