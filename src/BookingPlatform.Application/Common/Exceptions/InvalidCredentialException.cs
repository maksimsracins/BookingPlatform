namespace BookingPlatform.Application.Common.Exceptions;

public sealed class InvalidCredentialException : Exception
{
    public InvalidCredentialException()
        : base($"Invalid email or password.")
    {
    }
}