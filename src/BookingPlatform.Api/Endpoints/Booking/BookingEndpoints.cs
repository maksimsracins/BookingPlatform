using BookingPlatform.Application.Features.Booking.Create;
using FluentValidation;

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
}