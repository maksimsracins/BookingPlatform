namespace BookingPlatform.Application.Common.Exceptions;

public sealed class EmployeeNotFoundException : Exception
{
    public EmployeeNotFoundException(Guid id)
        : base($"Employee '{id}' not found.")
    {
    }
}