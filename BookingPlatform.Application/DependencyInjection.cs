using BookingPlatform.Application.Features.Booking.Create;
using Microsoft.Extensions.DependencyInjection;

namespace BookingPlatform.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<CreateBookingHandler>();

        return services;
    }
}