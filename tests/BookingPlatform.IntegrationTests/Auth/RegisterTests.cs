using System.Net.Http.Json;
using BookingPlatform.Application.Features.Auth.Register;
using BookingPlatform.Core.Entities;
using BookingPlatform.Core.Enums;
using BookingPlatform.IntegrationTests.Builders;
using BookingPlatform.IntegrationTests.Infrastructure;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace BookingPlatform.IntegrationTests.Auth;

public sealed class RegisterTests : IntegrationTest
{
        public RegisterTests(PostgreSqlFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task Should_Register_User()
    {
        var email = Guid.NewGuid() + "@test.com";
        var password = "TestPassword123!";
        var businessName = "Test Business " + Guid.NewGuid();

        var registerCommand = new RegisterCommand(email, password, businessName);

        var response = await Client.PostAsJsonAsync("/api/auth/register", registerCommand);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
    }

    [Fact]
    public async Task Should_Return_409_When_Email_Already_Exists()
    {
        var email = Guid.NewGuid() + "@test.com";
        var password = "TestPassword123!";
        var businessName = "Test Business " + Guid.NewGuid();

        var existingUser = User.Create(email, "hashed_password");
        Context.Users.Add(existingUser);
        await Context.SaveChangesAsync();

        var registerCommand = new RegisterCommand(email, password, businessName);
        var response = await Client.PostAsJsonAsync("/api/auth/register", registerCommand);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Conflict);
    }
}   