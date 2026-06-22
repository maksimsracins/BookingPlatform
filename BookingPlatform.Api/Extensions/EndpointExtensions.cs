using BookingPlatform.Api.Endpoints.Booking;

namespace BookingPlatform.Api.Extensions;

public static class EndpointExtensions
{
    public static WebApplication MapEndpoints(
        this WebApplication app)
    {
        app.MapBookingEndpoints();

        return app;
    }
}