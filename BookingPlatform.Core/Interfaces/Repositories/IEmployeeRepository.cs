using BookingPlatform.Core.Entities;
using BookingPlatform.Core.Interfaces.Repositories;

public interface IEmployeeRepository : IRepository<Employee>
{
    Task<IReadOnlyList<Employee>> GetByBusinessAsync(Guid businessId);

}