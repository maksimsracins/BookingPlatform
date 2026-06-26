using BookingPlatform.Core.Entities;

namespace BookingPlatform.IntegrationTests.Infrastructure;

public sealed record BookingData(
    Business Business,
    Employee Employee,
    Service Service,
    Client Client);