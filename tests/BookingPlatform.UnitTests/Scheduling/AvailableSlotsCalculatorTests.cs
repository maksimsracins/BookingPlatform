using BookingPlatform.Application.Common.Scheduling;
using BookingPlatform.Core.Entities;
using FluentAssertions;

public sealed class AvailableSlotsCalculatorTests
{
    private readonly AvailableSlotsCalculator _calculator = new();

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
            .BeEquivalentTo(
            [
                new TimeSpan(9,0,0),
                new TimeSpan(9,15,0),
                new TimeSpan(9,30,0)
            ]);
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