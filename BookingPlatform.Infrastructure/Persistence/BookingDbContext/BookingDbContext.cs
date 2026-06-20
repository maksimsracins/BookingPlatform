using BookingPlatform.Application.Common.Abstractions.Persistance;
using BookingPlatform.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingPlatform.Infrastructure.Persistence.Context;

public class BookingDbContext
    : DbContext,
      IBookingDbContext
{
    public DbSet<Business> Businesses => throw new NotImplementedException();

    public DbSet<Service> Services => throw new NotImplementedException();

    public DbSet<Employee> Employees => throw new NotImplementedException();

    public DbSet<Client> Clients => throw new NotImplementedException();

    public DbSet<Appointment> Appointments => throw new NotImplementedException();
}