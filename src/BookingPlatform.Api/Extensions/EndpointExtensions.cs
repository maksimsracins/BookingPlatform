using BookingPlatform.Api.Endpoints.Auth;
using BookingPlatform.Api.Endpoints.Booking;

namespace BookingPlatform.Api.Extensions;

public static class EndpointExtensions
{
    public static WebApplication MapEndpoints(
        this WebApplication app)
    {
        app.MapAuthEndpoints();
        app.MapBookingEndpoints();

        return app;
    }
}