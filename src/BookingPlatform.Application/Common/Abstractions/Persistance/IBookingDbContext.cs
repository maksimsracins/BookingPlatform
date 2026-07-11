using BookingPlatform.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingPlatform.Application.Common.Abstractions.Persistance;

public interface IBookingDbContext
{
    DbSet<Business> Businesses { get; }

    DbSet<Service> Services { get; }

    DbSet<Employee> Employees { get; }

    DbSet<Client> Clients { get; }

    DbSet<Appointment> Appointments { get; }

    DbSet<EmployeeWorkingHours> EmployeeWorkingHours { get; }

    DbSet<User> Users { get; }

    DbSet<BusinessUser> BusinessUsers { get; }

    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default);
}