using BookingPlatform.Application.Common.Authentication;
using MediatR;

public sealed record LoginCommand(string Email, string Password) : IRequest<AuthenticationResult>;