namespace BookingPlatform.Application.Common.Exceptions;

public sealed class EmployeeBusyException : Exception
{
    public EmployeeBusyException(Guid id)
        : base($"Employee '{id}' is busy.")
    {
    }
}