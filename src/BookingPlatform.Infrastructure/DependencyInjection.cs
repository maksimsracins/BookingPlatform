using BookingPlatform.Application.Authentication;
using BookingPlatform.Application.Common.Abstractions.Authentication;
using BookingPlatform.Application.Common.Abstractions.Persistance;
using BookingPlatform.Application.Common.Abstractions.Security;
using BookingPlatform.Application.Common.Scheduling;
using BookingPlatform.Infrastructure.Authentication;
using BookingPlatform.Infrastructure.Persistence.Context;
using BookingPlatform.Infrastructure.Security;
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
            configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' not found.");

        services.Configure<AuthenticationOptions>(configuration.GetSection(AuthenticationOptions.SectionName));

        services.AddSingleton<TimeProvider>(TimeProvider.System);
        
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();
        services.AddScoped<ITokenHasher, TokenHasher>();

        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.Configure<AuthenticationOptions>(configuration.GetSection(AuthenticationOptions.SectionName));

        services.AddDbContext<BookingDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        services.AddScoped<IBookingDbContext>(provider =>
            provider.GetRequiredService<BookingDbContext>());
        
        services.AddSingleton<AvailableSlotsCalculator>();
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, CurrentUser>();

        return services;
    }
}