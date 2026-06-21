using BookingPlatform.Application.Features.Booking.Create;

namespace BookingPlatform.Api.Endpoints.Booking;

public static class BookingEndpoints
{
    public static IEndpointRouteBuilder MapBookingEndpoints(
        this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/bookings", CreateBooking);

        return app;
    }

    private static async Task<IResult> CreateBooking(
        CreateBookingCommand command,
        CreateBookingHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(command, cancellationToken);

        return Results.Created(
            $"/api/bookings/{result.Id}",
            result);
    }
}