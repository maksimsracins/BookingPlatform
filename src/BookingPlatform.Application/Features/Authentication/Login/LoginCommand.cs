using BookingPlatform.Application.Features.Authentication.Login;
using MediatR;

public sealed record LoginCommand(string Email, string Password) : IRequest<LoginResponse>;