using MediatR;

namespace BookingPlatform.Application.Features.Auth.Register;

public sealed record RegisterCommand(
    string Email,
    string Password,
    string BusinessName)
    : IRequest<RegisterResponse>;