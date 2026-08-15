using BookingPlatform.Infrastructure.Persistence.Context;
using BookingPlatform.IntegrationTests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

public abstract class IntegrationTest : IClassFixture<PostgreSqlFixture>, IAsyncLifetime
{
    protected readonly HttpClient Client;

    protected readonly BookingDbContext Context;

    protected readonly TestData TestData;
    protected readonly PostgreSqlFixture Fixture;

    protected IntegrationTest(PostgreSqlFixture fixture)
    {
        Fixture = fixture;

        var factory = new CustomWebApplicationFactory(
            Fixture.ConnectionString);

        Client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost")
        });

        Context = factory.Services
            .CreateScope()
            .ServiceProvider
            .GetRequiredService<BookingDbContext>();

        TestData = new TestData(Context);
    }

    public Task InitializeAsync()
        => Fixture.ResetDatabaseAsync();

    public Task DisposeAsync()
        => Task.CompletedTask;
}