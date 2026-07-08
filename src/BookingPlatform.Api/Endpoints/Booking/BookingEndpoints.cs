using BookingPlatform.Application.Features.Booking.Create;
using BookingPlatform.Application.Features.Booking.GetAvailableSlots;
using BookingPlatform.Application.Features.Business.GetBusinesses;
using FluentValidation;
using MediatR;

namespace BookingPlatform.Api.Endpoints.Booking;

public static class BookingEndpoints
{
    public static IEndpointRouteBuilder MapBookingEndpoints(
        this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/bookings", CreateBooking);
        app.MapGet("/api/bookings/available-slots", GetAvailableSlots);
        app.MapGet("/api/businesses", GetBusinesses);

        return app;
    }

    private static async Task<IResult> CreateBooking(
        CreateBookingCommand command,
        IMediator mediator,
        IValidator<CreateBookingCommand> validator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);

        return Results.Created(
            $"/api/bookings/{result.Id}",
            result);
    }

     private static async Task<IResult> GetAvailableSlots(
        [AsParameters] GetAvailableSlotsQuery query, 
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            query,
            cancellationToken);

        return Results.Ok(result);
    }

    private static async Task<IResult> GetBusinesses(IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetBusinessesQuery(), cancellationToken);

        return Results.Ok(result);
    }
}