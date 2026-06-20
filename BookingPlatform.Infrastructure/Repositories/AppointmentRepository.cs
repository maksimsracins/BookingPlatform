using BookingPlatform.Application.Interfaces.Repositories;
using BookingPlatform.Core.Entities;
using BookingPlatform.Infrastructure.Persistence.Context;
using BookingPlatform.Infrastructure.Repositories;

namespace BookingPlatform.Infrastructure.Repositories;s

public class AppointmentRepository
    : Repository<Appointment>,
      IAppointmentRepository
{
    public AppointmentRepository(
        BookingDbContext context)
        : base(context)
    {
    }

    public Task<bool> ExistsAsync(Guid employeeId, DateTime start, DateTime end)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<Appointment>> GetByDateAsync(Guid businessId, DateOnly date)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<Appointment>> GetByEmployeeAsync(Guid employeeId)
    {
        throw new NotImplementedException();
    }
}