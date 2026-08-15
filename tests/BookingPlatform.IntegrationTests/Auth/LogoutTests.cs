using System.Net;
using System.Net.Http.Headers;
using BookingPlatform.Api.Authentication;
using BookingPlatform.IntegrationTests.Infrastructure;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace BookingPlatform.IntegrationTests.Auth;

public class LogoutTests : IntegrationTest
{
    private readonly AuthenticationTestHelper _authHelper;

    public LogoutTests(PostgreSqlFixture fixture)
        : base(fixture)
    {
         _authHelper = new AuthenticationTestHelper(Client);
    }

    [Fact]
    public async Task Should_Logout()
    {
        await _authHelper.RegisterAsync();

        await _authHelper.LoginAsync();

        var response = await Client.PostAsync(
            "/api/auth/logout",
            null);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var refresh = await Context.RefreshTokens.SingleAsync();

        refresh.RevokedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task Should_Be_Able_To_Logout_Twice()
    {
        await _authHelper.RegisterAsync();

        await _authHelper.LoginAsync();

        (await Client.PostAsync("/api/auth/logout", null))
            .StatusCode
            .Should()
            .Be(HttpStatusCode.NoContent);

        (await Client.PostAsync("/api/auth/logout", null))
            .StatusCode
            .Should()
            .Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Should_Not_Refresh_After_Logout()
    {
        await _authHelper.RegisterAsync();

        await _authHelper.LoginAsync();

        await Client.PostAsync(
            "/api/auth/logout",
            null);

        var response = await Client.PostAsync(
            "/api/auth/refresh",
            null);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Should_Return_204_When_Refresh_Token_Does_Not_Exist()
    {
        Client.DefaultRequestHeaders.Add(
            "Cookie",
            $"{CookieNames.RefreshToken}=fake-refresh-token");

        var response = await Client.PostAsync(
            "/api/auth/logout",
            null);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        (await Context.RefreshTokens.CountAsync())
            .Should()
            .Be(0);
    }
}