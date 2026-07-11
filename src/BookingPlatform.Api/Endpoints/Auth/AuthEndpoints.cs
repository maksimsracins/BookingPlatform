using BookingPlatform.Application.Features.Auth.Register;
using MediatR;

namespace BookingPlatform.Api.Endpoints.Auth;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/register", Register);

        return app;
    }

    private static async Task<IResult> Register(RegisterCommand command, ISender sender, CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            command,
            cancellationToken);

        return Results.Created(
            $"/api/users/{result.UserId}",
            result);
    }
}