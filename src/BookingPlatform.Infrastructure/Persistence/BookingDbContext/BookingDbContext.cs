using BookingPlatform.Application.Common.Abstractions.Persistance;
using BookingPlatform.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingPlatform.Infrastructure.Persistence.Context;

public class BookingDbContext
    : DbContext,
      IBookingDbContext
{
    public BookingDbContext(DbContextOptions<BookingDbContext> options)
        : base(options)
    {
    }

    public DbSet<Business> Businesses { get; set; } = null!;

    public DbSet<Service> Services { get; set; } = null!;

    public DbSet<Employee> Employees { get; set; } = null!;

    public DbSet<Client> Clients { get; set; } = null!;

    public DbSet<Appointment> Appointments { get; set; } = null!;
}