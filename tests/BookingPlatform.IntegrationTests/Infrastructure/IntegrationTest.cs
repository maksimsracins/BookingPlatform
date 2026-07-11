using BookingPlatform.Infrastructure.Persistence.Context;
using BookingPlatform.IntegrationTests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

public abstract class IntegrationTest : IClassFixture<PostgreSqlFixture>
{
    protected readonly HttpClient Client;

    protected readonly BookingDbContext Context;

    protected readonly TestData TestData;

    protected IntegrationTest(PostgreSqlFixture fixture)
    {
        var factory = new CustomWebApplicationFactory(
            fixture.ConnectionString);

        Client = factory.CreateClient();

        Context = factory.Services
            .CreateScope()
            .ServiceProvider
            .GetRequiredService<BookingDbContext>();

        TestData = new TestData(Context);
    }
}