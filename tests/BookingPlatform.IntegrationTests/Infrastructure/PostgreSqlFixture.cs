using System.Net;
using BookingPlatform.Application.Features.Authentication.Login;
using BookingPlatform.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Respawn;
using Testcontainers.PostgreSql;

namespace BookingPlatform.IntegrationTests.Infrastructure;

public sealed class PostgreSqlFixture : IAsyncLifetime
{

    private readonly PostgreSqlContainer _container =
        new PostgreSqlBuilder()
            .WithImage("postgres:17")
            .WithDatabase("booking")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();

    private NpgsqlConnection _connection = default!;
    private Respawner _respawner = default!;

    public string ConnectionString
        => _container.GetConnectionString();

    public async Task InitializeAsync()
    {
        await _container.StartAsync();

        var options = new DbContextOptionsBuilder<BookingDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        using (var context = new BookingDbContext(options))
        {
            await context.Database.MigrateAsync();
        }

        _connection = new NpgsqlConnection(ConnectionString);
        await _connection.OpenAsync();

        _respawner = await Respawner.CreateAsync(
            _connection,
            new RespawnerOptions
            {
                DbAdapter = DbAdapter.Postgres
            });
    }

    public Task ResetDatabaseAsync()
    {
        return _respawner.ResetAsync(_connection);
    }

    public async Task DisposeAsync()
    {
        await _connection.DisposeAsync();
        await _container.DisposeAsync();
    }
}