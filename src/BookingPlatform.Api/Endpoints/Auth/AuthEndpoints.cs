using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BookingPlatform.Api.Authentication;
using BookingPlatform.Application.Features.Auth.Register;
using BookingPlatform.Application.Features.Authentication.Refresh;
using MediatR;

namespace BookingPlatform.Api.Endpoints.Auth;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/register", Register);
        app.MapPost("/api/auth/login", Login);
        app.MapPost("/api/auth/refresh", Refresh);
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

    private static async Task<IResult> Login(
        LoginCommand command, 
        ISender sender, 
        HttpContext httpContext, 
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            command,
            cancellationToken);

        httpContext.Response.Cookies.Append(
            CookieNames.RefreshToken,
            result.RefreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = result.AccessToken.ExpiresAt.AddDays(30)
            });

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

    private static async Task<IResult> Refresh(HttpContext httpContext, ISender sender, CancellationToken cancellationToken)
    {
        if (!httpContext.Request.Cookies.TryGetValue(
                CookieNames.RefreshToken,
                out var refreshToken))
            {
                return Results.Unauthorized();
            }

        var result = await sender.Send(
            new RefreshCommand(refreshToken),
            cancellationToken);

        httpContext.Response.Cookies.Append(CookieNames.RefreshToken, result.RefreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

        return Results.Ok(new
        {
            accessToken = result.AccessToken.AccessToken,
            expiresAt = result.AccessToken.ExpiresAt
        });
    }
}