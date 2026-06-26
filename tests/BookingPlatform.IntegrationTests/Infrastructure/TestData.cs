using BookingPlatform.Core.Entities;
using BookingPlatform.Infrastructure.Persistence.Context;
using BookingPlatform.IntegrationTests.Builders;

namespace BookingPlatform.IntegrationTests.Infrastructure;

public sealed class TestData
{
    private readonly BookingDbContext _context;

    public TestData(BookingDbContext context)
    {
        _context = context;
    }

    public async Task<BookingData> SeedBookingAsync()
    {
        var business = new BusinessBuilder().Build();

        var employee = new EmployeeBuilder()
            .ForBusiness(business.Id)
            .Build();

        var service = new ServiceBuilder()
            .ForBusiness(business.Id)
            .Build();

        var client = new ClientBuilder()
            .ForBusiness(business.Id)
            .Build();

        _context.Businesses.Add(business);
        _context.Employees.Add(employee);
        _context.Services.Add(service);
        _context.Clients.Add(client);

        await _context.SaveChangesAsync();

        return new BookingData(
            business,
            employee,
            service,
            client);
    }
}