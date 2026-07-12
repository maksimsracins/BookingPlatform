using BookingPlatform.Application.Features.Authentication.GetCurrentUser;
using MediatR;

public sealed record GetCurrentUserQuery() : IRequest<GetCurrentUserResponse>;