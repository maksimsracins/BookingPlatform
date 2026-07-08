using BookingPlatform.Application.Common.Behaviors;
using BookingPlatform.Application.Features.Booking.Create;
using BookingPlatform.Application.Features.Booking.GetAvailableSlots;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BookingPlatform.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);

            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        services.AddValidatorsFromAssemblyContaining<CreateBookingValidator>();

        return services;
    }
}