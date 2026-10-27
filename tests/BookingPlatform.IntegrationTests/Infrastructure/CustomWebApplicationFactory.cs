using BookingPlatform.Application.Common.Abstractions.Persistance;
using BookingPlatform.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BookingPlatform.IntegrationTests.Infrastructure;

public sealed class CustomWebApplicationFactory
    : WebApplicationFactory<Program>
{
    private readonly string _connectionString;

    public CustomWebApplicationFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // UseSetting applies before Program reads configuration; ConfigureAppConfiguration runs too late for AddJwtAuthentication.
        builder.UseSetting("Jwt:SecretKey", "integration-test-only-signing-key-32-bytes");

        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(x =>
                x.ServiceType == typeof(DbContextOptions<BookingDbContext>));

            if (descriptor != null)
                services.Remove(descriptor);

            services.AddDbContext<BookingDbContext>(options =>
            {
                options.UseNpgsql(_connectionString);
            });

            services.AddScoped<IBookingDbContext>(sp =>
                sp.GetRequiredService<BookingDbContext>());
        });
    }
}