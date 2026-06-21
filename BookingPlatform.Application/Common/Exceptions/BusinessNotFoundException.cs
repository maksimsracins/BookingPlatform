namespace BookingPlatform.Application.Common.Exceptions;

public sealed class BusinessNotFoundException : Exception
{
    public BusinessNotFoundException(Guid id)
        : base($"Business '{id}' not found.")
    {
    }
}