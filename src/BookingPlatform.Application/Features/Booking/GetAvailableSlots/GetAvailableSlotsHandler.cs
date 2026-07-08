namespace BookingPlatform.Application.Features.Booking.GetAvailableSlots;

using BookingPlatform.Application.Common.Abstractions.Persistance;
using BookingPlatform.Application.Common.Exceptions;
using BookingPlatform.Application.Common.Scheduling;
using BookingPlatform.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

public sealed class GetAvailableSlotsHandler(
    IBookingDbContext context,
    AvailableSlotsCalculator calculator) : IRequestHandler<GetAvailableSlotsQuery, GetAvailableSlotsResult>
{
    private readonly IBookingDbContext _context = context;
    private readonly AvailableSlotsCalculator _calculator = calculator;

    public async Task<GetAvailableSlotsResult> Handle(
        GetAvailableSlotsQuery query,
        CancellationToken cancellationToken)
    {

        var business = await _context.Businesses
            .FirstOrDefaultAsync(x => x.Id == query.BusinessId, cancellationToken)
            ?? throw new BusinessNotFoundException(query.BusinessId);

        var service = await _context.Services
            .FirstOrDefaultAsync(x =>
                x.Id == query.ServiceId &&
                x.BusinessId == query.BusinessId,
                cancellationToken)
            ?? throw new ServiceNotFoundException(query.ServiceId);

        var workingHours = await _context.EmployeeWorkingHours
            .FirstOrDefaultAsync(x =>
                x.EmployeeId == query.EmployeeId &&
                x.DayOfWeek == query.Date.DayOfWeek,
                cancellationToken);

        if (workingHours is null)
            return new GetAvailableSlotsResult([]);

        var dateStartUtc = DateTime.SpecifyKind(query.Date.ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc);
        var dateEndUtc = dateStartUtc.AddDays(1);

        var appointments = await _context.Appointments
            .Where(x =>
                x.EmployeeId == query.EmployeeId &&
                x.StartAt >= dateStartUtc &&
                x.StartAt < dateEndUtc)
            .OrderBy(x => x.StartAt)
            .ToListAsync(cancellationToken);

        var slots = _calculator.Calculate(
            workingHours.StartTime,
            workingHours.EndTime,
            service.Duration,
            business.SlotInterval,
            appointments,
            query.Date);

        return new GetAvailableSlotsResult(
            slots.Select(x => new AvailableSlotDto(
                x.StartAt,
                x.EndAt))
            .ToList());
    }
}