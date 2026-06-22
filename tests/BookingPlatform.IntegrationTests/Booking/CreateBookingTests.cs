using System.Net;
using System.Net.Http.Json;
using BookingPlatform.Application.Features.Booking.Create;
using BookingPlatform.IntegrationTests.Infrastructure;
using FluentAssertions;

namespace BookingPlatform.IntegrationTests.Booking;

public sealed class CreateBookingTests
    : IntegrationTest
{
    public CreateBookingTests(PostgreSqlFixture fixture)
        : base(fixture)
    {
    }

    [Fact]
    public async Task Should_Return404_WhenBusinessDoesNotExist()
    {
        var command = new CreateBookingCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            DateTime.UtcNow.AddHours(2));

        var response = await Client.PostAsJsonAsync(
            "/api/bookings",
            command);

        response.StatusCode.Should()
            .Be(HttpStatusCode.NotFound);
    }
}