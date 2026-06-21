namespace BookingPlatform.Application.Common.Exceptions;

public sealed class ClientNotFoundException : Exception
{
    public ClientNotFoundException(Guid id)
        : base($"Client '{id}' not found.")
    {
    }
}