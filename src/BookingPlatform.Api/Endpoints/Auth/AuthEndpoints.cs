using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BookingPlatform.Application.Features.Auth.Register;
using MediatR;

namespace BookingPlatform.Api.Endpoints.Auth;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/register", Register);
        app.MapPost("/api/auth/login", Login);
        app.MapGet(
            "/api/auth/me",
            async (
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    new GetCurrentUserQuery(),
                    cancellationToken);

                return Results.Ok(result);
            })
        .RequireAuthorization();

        return app;
    }

    private static async Task<IResult> Login(LoginCommand command, ISender sender, CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            command,
            cancellationToken);

        return Results.Ok(result);
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