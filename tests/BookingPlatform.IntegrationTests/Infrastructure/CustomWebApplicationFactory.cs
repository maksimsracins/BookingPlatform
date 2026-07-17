using BookingPlatform.Application.Common.Abstractions.Persistance;
using BookingPlatform.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ApiProgram = BookingPlatform.Api.Program;

namespace BookingPlatform.IntegrationTests.Infrastructure;

public sealed class CustomWebApplicationFactory : WebApplicationFactory<ApiProgram>
{
    private readonly string _connectionString;

    public CustomWebApplicationFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
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