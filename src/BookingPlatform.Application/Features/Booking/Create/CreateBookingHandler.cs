using BookingPlatform.Application.Common.Abstractions.Persistance;
using BookingPlatform.Application.Common.Exceptions;
using BookingPlatform.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingPlatform.Application.Features.Booking.Create;

public sealed class CreateBookingHandler(IBookingDbContext context) : IRequestHandler<CreateBookingCommand, CreateBookingResult>
{
    private readonly IBookingDbContext _context = context;

    public async Task<CreateBookingResult> Handle(
        CreateBookingCommand command,
        CancellationToken cancellationToken)
    {
        var business = await GetBusinessAsync(command.BusinessId, cancellationToken);

        var service = await GetServiceAsync(
            business.Id,
            command.ServiceId,
            cancellationToken);

        var employee = await GetEmployeeAsync(
            business.Id,
            command.EmployeeId,
            cancellationToken);

        var client = await GetClientAsync(
            business.Id,
            command.ClientId,
            cancellationToken);

        await EnsureEmployeeIsAvailable(
            employee.Id,
            command.StartAt,
            service.Duration,
            cancellationToken);

        var appointment = CreateAppointment(
            business,
            service,
            employee,
            client,
            command);

        _context.Appointments.Add(appointment);

        await _context.SaveChangesAsync(cancellationToken);

        return new CreateBookingResult(appointment.Id);
    }

    private async Task<BookingPlatform.Core.Entities.Business> GetBusinessAsync(
        Guid businessId,
        CancellationToken cancellationToken)
    {
        return await _context.Businesses
            .FirstOrDefaultAsync(x => x.Id == businessId, cancellationToken)
            ?? throw new BusinessNotFoundException(businessId);
    }

    private async Task<Service> GetServiceAsync(
        Guid businessId,
        Guid serviceId,
        CancellationToken cancellationToken)
    {
        return await _context.Services
            .FirstOrDefaultAsync(x =>
                    x.Id == serviceId &&
                    x.BusinessId == businessId &&
                    x.IsActive,
                cancellationToken)
            ?? throw new ServiceNotFoundException(serviceId);
    }

    private async Task<Employee> GetEmployeeAsync(
        Guid businessId,
        Guid employeeId,
        CancellationToken cancellationToken)
    {
        return await _context.Employees
            .FirstOrDefaultAsync(x =>
                    x.Id == employeeId &&
                    x.BusinessId == businessId &&
                    x.IsActive,
                cancellationToken)
            ?? throw new EmployeeNotFoundException(employeeId);
    }

    private async Task<Client> GetClientAsync(
        Guid businessId,
        Guid clientId,
        CancellationToken cancellationToken)
    {
        return await _context.Clients
            .FirstOrDefaultAsync(x =>
                    x.Id == clientId &&
                    x.BusinessId == businessId,
                cancellationToken)
            ?? throw new ClientNotFoundException(clientId);
    }

    private async Task EnsureEmployeeIsAvailable(
        Guid employeeId,
        DateTime startAt,
        TimeSpan duration,
        CancellationToken cancellationToken)
    {
        var endAt = startAt.Add(duration);

        var exists = await _context.Appointments.AnyAsync(x =>
                x.EmployeeId == employeeId &&
                x.StartAt < endAt &&
                x.EndAt > startAt,
            cancellationToken);

        if (exists)
            throw new EmployeeBusyException(employeeId);
    }

    private static Appointment CreateAppointment(
        BookingPlatform.Core.Entities.Business business,
        Service service,
        Employee employee,
        Client client,
        CreateBookingCommand command)
    {
        var endAt = command.StartAt.Add(service.Duration);

        return new Appointment(
            Guid.NewGuid(),
            business.Id,
            client.Id,
            employee.Id,
            service.Id,
            service.Price,
            service.Duration,
            command.StartAt,
            endAt);
    }
}