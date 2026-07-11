namespace BookingPlatform.Application.Common.Exceptions;

public sealed class EmailAlreadyExistException : Exception
{
    public EmailAlreadyExistException(string email)
        : base($"User with email '{email}' already exist.")
    {
    }
}