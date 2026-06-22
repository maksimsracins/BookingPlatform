using System.Net.Http;
using BookingPlatform.Infrastructure.Persistence.Context;
using Microsoft.Extensions.DependencyInjection;

namespace BookingPlatform.IntegrationTests.Infrastructure;

public abstract class IntegrationTest
    : IClassFixture<PostgreSqlFixture>
{
    protected readonly HttpClient Client;

    protected readonly BookingDbContext Context;

    protected IntegrationTest(PostgreSqlFixture fixture)
    {
        var factory = new CustomWebApplicationFactory(
            fixture.ConnectionString);

        Client = factory.CreateClient();

        Context = factory.Services
            .CreateScope()
            .ServiceProvider
            .GetRequiredService<BookingDbContext>();
    }
}