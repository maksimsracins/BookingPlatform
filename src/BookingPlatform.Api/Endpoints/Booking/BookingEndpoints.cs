using BookingPlatform.Application.Features.Booking.Create;
using BookingPlatform.Application.Features.Booking.GetAvailableSlots;
using FluentValidation;

namespace BookingPlatform.Api.Endpoints.Booking;

public static class BookingEndpoints
{
    public static IEndpointRouteBuilder MapBookingEndpoints(
        this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/bookings", CreateBooking);
        app.MapGet("/api/bookings/available-slots", GetAvailableSlots);

        return app;
    }

    private static async Task<IResult> CreateBooking(
        CreateBookingCommand command,
        CreateBookingHandler handler,
        IValidator<CreateBookingCommand> validator,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var result = await handler.Handle(command, cancellationToken);

        return Results.Created(
            $"/api/bookings/{result.Id}",
            result);
    }

     private static async Task<IResult> GetAvailableSlots(
        [AsParameters] GetAvailableSlotsQuery query, 
        GetAvailableSlotsHandler handler, 
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            query,
            cancellationToken);

        return Results.Ok(result);
    }
}