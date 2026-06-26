using System.Data;
using System.Net;
using System.Net.Http.Json;
using BookingPlatform.Application.Features.Booking.Create;
using BookingPlatform.Core.Entities;
using BookingPlatform.IntegrationTests.Builders;
using BookingPlatform.IntegrationTests.Infrastructure;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace BookingPlatform.IntegrationTests.Booking;

public sealed class CreateBookingTests : IntegrationTest
{
    public CreateBookingTests(PostgreSqlFixture fixture)
        : base(fixture)
    {
    }

    [Fact]
    public async Task Should_Create_Booking()
    {
        var data = await TestData.SeedBookingAsync();

        var command = new CreateBookingCommand(
            data.Business.Id,
            data.Service.Id,
            data.Employee.Id,
            data.Client.Id,
            DateTime.UtcNow.AddHours(2));

        var response = await Client.PostAsJsonAsync(
            "/api/bookings",
            command);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var appointment = await Context.Appointments.SingleAsync();

        appointment.BusinessId.Should().Be(data.Business.Id);
        appointment.ClientId.Should().Be(data.Client.Id);
        appointment.EmployeeId.Should().Be(data.Employee.Id);
        appointment.ServiceId.Should().Be(data.Service.Id);
    }

    [Fact]
    public async Task Should_Return_404_When_Business_Does_Not_Exist()
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

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Should_Return_409_When_Employee_Is_Busy()
    {
        var data = await TestData.SeedBookingAsync();

        var appointment = new AppointmentBuilder()
            .ForBusiness(data.Business.Id)
            .ForClient(data.Client.Id)
            .ForEmployee(data.Employee.Id)
            .ForService(data.Service.Id)
            .StartingAt(DateTime.UtcNow.AddHours(2).AddMinutes(10))
            .WithPrice(data.Service.Price)
            .WithDuration(data.Service.Duration)
            .Build();

        await Context.Appointments.AddAsync(appointment);
        await Context.SaveChangesAsync();

        var command = new CreateBookingCommand(
            data.Business.Id,
            data.Service.Id,
            data.Employee.Id,
            data.Client.Id,
            DateTime.UtcNow.AddHours(2).AddMinutes(39));

        var response = await Client.PostAsJsonAsync(
            "/api/bookings",
            command);

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Should_Return_201_When_Booking_Created()
    {
        var data = await TestData.SeedBookingAsync();

        var appointment = new AppointmentBuilder()
            .ForBusiness(data.Business.Id)
            .ForClient(data.Client.Id)
            .ForEmployee(data.Employee.Id)
            .ForService(data.Service.Id)
            .StartingAt(DateTime.UtcNow.AddHours(2).AddMinutes(10))
            .WithPrice(data.Service.Price)
            .WithDuration(data.Service.Duration)
            .Build();

            await Context.Appointments.AddAsync(appointment);
            await Context.SaveChangesAsync();

        var command = new CreateBookingCommand(
            data.Business.Id,
            data.Service.Id,
            data.Employee.Id,
            data.Client.Id,
            DateTime.UtcNow.AddHours(2).AddMinutes(40));

        var response = await Client.PostAsJsonAsync(
            "/api/bookings",
            command);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Should_Return_404_When_Client_Does_Not_Exist()
    {
        var data = await TestData.SeedBookingAsync();

        var command = new CreateBookingCommand(
            data.Business.Id,
            data.Service.Id,
            data.Employee.Id,
            Guid.NewGuid(),
            DateTime.UtcNow.AddHours(2));

        var response = await Client.PostAsJsonAsync(
            "/api/bookings",
            command);
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Should_Return_404_When_Service_Does_Not_Exist()
    {
        var data = await TestData.SeedBookingAsync();

        var command = new CreateBookingCommand(
            data.Business.Id,
            Guid.NewGuid(),
            data.Employee.Id,
            data.Client.Id,
            DateTime.UtcNow.AddHours(2));

        var response = await Client.PostAsJsonAsync(
            "/api/bookings",
            command);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Should_Return_404_When_Employee_Does_Not_Exist()
    {
        var data = await TestData.SeedBookingAsync();

        var command = new CreateBookingCommand(
            data.Business.Id,
            data.Service.Id,
            Guid.NewGuid(),
            data.Client.Id,
            DateTime.UtcNow.AddHours(2));

        var response = await Client.PostAsJsonAsync(
            "/api/bookings",
            command);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Should_Return_400_When_DateTime_Negative()
    {
        var data = await TestData.SeedBookingAsync();

        var command = new CreateBookingCommand(
            data.Business.Id,
            data.Service.Id,
            data.Employee.Id,
            data.Client.Id,
            DateTime.UtcNow.AddHours(-2));

        var response = await Client.PostAsJsonAsync(
            "/api/bookings",
            command
        );

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Should_Return_400_When_Business_Empty()
    {
        var data = await TestData.SeedBookingAsync();

        var command = new CreateBookingCommand(
            Guid.Empty,
            data.Service.Id,
            data.Employee.Id,
            data.Client.Id,
            DateTime.UtcNow.AddHours(2));

        var response = await Client.PostAsJsonAsync(
            "/api/bookings",
            command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}