using BookingPlatform.Application.Common.Authentication;
using MediatR;

namespace BookingPlatform.Application.Features.Authentication.Refresh;

public sealed record RefreshCommand(string RefreshToken) : IRequest<AuthenticationResult>;