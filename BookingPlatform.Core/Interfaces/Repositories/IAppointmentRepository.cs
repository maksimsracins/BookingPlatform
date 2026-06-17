using BookingPlatform.Core.Entities;
using BookingPlatform.Core.Interfaces.Repositories;

public interface IAppointmentRepository : IRepository<Appointment>
{
    Task<IReadOnlyList<Appointment>> GetByEmployeeAsync(Guid employeeId);

    Task<IReadOnlyList<Appointment>> GetByDateAsync(Guid businessId, DateOnly date);

    Task<bool> ExistsAsync(
        Guid employeeId,
        DateTime start,
        DateTime end);

}