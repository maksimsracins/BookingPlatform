using System.Net;
using System.Net.Http.Json;
using BookingPlatform.Application.Common.Scheduling;
using BookingPlatform.Application.Features.Booking.GetAvailableSlots;
using BookingPlatform.IntegrationTests.Builders;
using BookingPlatform.IntegrationTests.Infrastructure;
using FluentAssertions;

namespace BookingPlatform.IntegrationTests.Booking;

public sealed class GetAvailableSlotsTests : IntegrationTest
{
    public GetAvailableSlotsTests(PostgreSqlFixture fixture)
        : base(fixture)
    {
    }

    private readonly AvailableSlotsCalculator _calculator = new();
    

    [Fact]
    public async Task Should_Return_Available_Slots()
    {
        var business = new BusinessBuilder().Build();

        var employee = new EmployeeBuilder()
            .ForBusiness(business.Id)
            .Build();

        var service = new ServiceBuilder()
            .ForBusiness(business.Id)
            .WithDuration(TimeSpan.FromMinutes(30))
            .Build();

        var client = new ClientBuilder()
            .ForBusiness(business.Id)
            .Build();

        var workingHours = new EmployeeWorkingHoursBuilder()
            .ForEmployee(employee.Id)
            .ForDay(DayOfWeek.Monday)
            .From(new TimeOnly(9, 0))
            .To(new TimeOnly(12, 0))
            .Build();

        var date = new DateOnly(2026, 6, 29); // Monday

        var appointment = new AppointmentBuilder()
            .ForBusiness(business.Id)
            .ForEmployee(employee.Id)
            .ForClient(client.Id)
            .ForService(service.Id)
            .WithPrice(service.Price)
            .WithDuration(service.Duration)
            .StartingAt(date.ToDateTime(new TimeOnly(10, 0)))
            .Build();

        Context.Businesses.Add(business);
        Context.Employees.Add(employee);
        Context.Services.Add(service);
        Context.Clients.Add(client);
        Context.EmployeeWorkingHours.Add(workingHours);
        Context.Appointments.Add(appointment);

        await Context.SaveChangesAsync();

        var response = await Client.GetAsync(
            $"/api/bookings/available-slots" +
            $"?businessId={business.Id}" +
            $"&employeeId={employee.Id}" +
            $"&serviceId={service.Id}" +
            $"&date={date:yyyy-MM-dd}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content
            .ReadFromJsonAsync<GetAvailableSlotsResult>();

        result.Should().NotBeNull();

        result!.Slots.Should().NotBeEmpty();

        result.Slots.Should().NotContain(x =>
            x.StartAt == date.ToDateTime(new TimeOnly(10, 0)));

        result.Slots.Should().NotContain(x =>
            x.StartAt == date.ToDateTime(new TimeOnly(10, 15)));

        result.Slots.Should().Contain(x =>
            x.StartAt == date.ToDateTime(new TimeOnly(9, 0)));

        result.Slots.Should().Contain(x =>
            x.StartAt == date.ToDateTime(new TimeOnly(11, 0)));
    }

    [Fact]
    public void Should_Return_All_Slots_When_No_Appointments()
    {
        var date = new DateOnly(2026, 6, 29);

        var slots = _calculator.Calculate(
            new TimeOnly(9, 0),
            new TimeOnly(10, 0),
            TimeSpan.FromMinutes(30),
            TimeSpan.FromMinutes(15),
            [],
            date);

        slots.Should().HaveCount(3);

        slots.Select(x => x.StartAt.TimeOfDay)
            .Should()
            .Equal(
                new TimeSpan(9, 0, 0),
                new TimeSpan(9, 15, 0),
                new TimeSpan(9, 30, 0));
    }

    [Fact]
    public void Should_Exclude_Busy_Slot()
    {
        var date = new DateOnly(2026, 6, 29);

        var appointment = new AppointmentBuilder()
            .StartingAt(date.ToDateTime(new TimeOnly(9, 30)))
            .WithDuration(TimeSpan.FromMinutes(30))
            .Build();

        var slots = _calculator.Calculate(
            new TimeOnly(9, 0),
            new TimeOnly(10, 30),
            TimeSpan.FromMinutes(30),
            TimeSpan.FromMinutes(15),
            [appointment],
            date);

        slots.Should().Contain(x =>
            x.StartAt == date.ToDateTime(new TimeOnly(9, 0)));

        slots.Should().NotContain(x =>
            x.StartAt == date.ToDateTime(new TimeOnly(9, 15)));

        slots.Should().NotContain(x =>
            x.StartAt == date.ToDateTime(new TimeOnly(9, 30)));
    }

    [Fact]
    public void Should_Exclude_Overlapping_Slots()
    {
        var date = new DateOnly(2026, 6, 29);

        var appointment = new AppointmentBuilder()
            .StartingAt(date.ToDateTime(new TimeOnly(9, 10)))
            .WithDuration(TimeSpan.FromMinutes(30))
            .Build();

        var slots = _calculator.Calculate(
            new TimeOnly(9, 0),
            new TimeOnly(10, 0),
            TimeSpan.FromMinutes(30),
            TimeSpan.FromMinutes(15),
            [appointment],
            date);

        slots.Should().BeEmpty();
    }

    [Fact]
    public void Should_Not_Return_Slots_Outside_Working_Hours()
    {
        var date = new DateOnly(2026, 6, 29);

        var slots = _calculator.Calculate(
            new TimeOnly(9, 0),
            new TimeOnly(10, 0),
            TimeSpan.FromMinutes(45),
            TimeSpan.FromMinutes(15),
            [],
            date);

        slots.Should().HaveCount(2);

        slots.Should().Contain(x =>
            x.StartAt == date.ToDateTime(new TimeOnly(9, 0)));

        slots.Should().Contain(x =>
            x.StartAt == date.ToDateTime(new TimeOnly(9, 15)));

        slots.Should().NotContain(x =>
            x.StartAt == date.ToDateTime(new TimeOnly(9, 30)));
    }

    [Fact]
    public void Should_Return_Empty_When_Working_Day_Is_Shorter_Than_Service()
    {
        var date = new DateOnly(2026, 6, 29);

        var slots = _calculator.Calculate(
            new TimeOnly(9, 0),
            new TimeOnly(10, 0),
            TimeSpan.FromHours(2),
            TimeSpan.FromMinutes(15),
            [],
            date);

        slots.Should().BeEmpty();
    }

    [Fact]
    public void Should_Return_Slots_After_Last_Appointment()
    {
        var date = new DateOnly(2026, 6, 29);

        var appointment = new AppointmentBuilder()
            .StartingAt(date.ToDateTime(new TimeOnly(9, 0)))
            .WithDuration(TimeSpan.FromHours(1))
            .Build();

        var slots = _calculator.Calculate(
            new TimeOnly(9, 0),
            new TimeOnly(12, 0),
            TimeSpan.FromMinutes(30),
            TimeSpan.FromMinutes(15),
            [appointment],
            date);

        slots.Should().Contain(x =>
            x.StartAt == date.ToDateTime(new TimeOnly(10, 0)));

        slots.Should().Contain(x =>
            x.StartAt == date.ToDateTime(new TimeOnly(10, 15)));
    }

    [Fact]
    public void Should_Return_Empty_When_Working_Day_Is_Fully_Booked()
    {
        var date = new DateOnly(2026, 6, 29);

        var appointment = new AppointmentBuilder()
            .StartingAt(date.ToDateTime(new TimeOnly(9, 0)))
            .WithDuration(TimeSpan.FromHours(9))
            .Build();

        var slots = _calculator.Calculate(
            new TimeOnly(9, 0),
            new TimeOnly(18, 0),
            TimeSpan.FromMinutes(30),
            TimeSpan.FromMinutes(15),
            [appointment],
            date);

        slots.Should().BeEmpty();
    }

}