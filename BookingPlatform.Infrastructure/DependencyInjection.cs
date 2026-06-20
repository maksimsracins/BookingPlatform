using BookingPlatform.Application.Common.Abstractions.Persistance;
using BookingPlatform.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookingPlatform.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' not found.");

        services.AddDbContext<BookingDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        services.AddScoped<IBookingDbContext>(provider =>
            provider.GetRequiredService<BookingDbContext>());

        return services;
    }
}