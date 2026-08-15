using System.Net;
using System.Net.Http.Json;
using BookingPlatform.Api.Authentication;
using BookingPlatform.IntegrationTests.Auth;
using BookingPlatform.IntegrationTests.Infrastructure;
using FluentAssertions;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;

public class LoginTests : IntegrationTest
{
    private readonly AuthenticationTestHelper _authHelper;

    public LoginTests(PostgreSqlFixture fixture)
        : base(fixture)
    {
         _authHelper = new AuthenticationTestHelper(Client);
    }

    [Fact]
    public async Task Should_Login_With_Valid_Credentials()
    {
        await _authHelper.RegisterAsync();

        var login = await _authHelper.LoginAsync();

        login.AccessToken.AccessToken.Should().NotBeNullOrWhiteSpace();

        Context.RefreshTokens.Should().ContainSingle();
    }

    [Fact]
    public async Task Should_Return_401_When_Email_Does_Not_Exist()
    {
        var response = await Client.PostAsJsonAsync(
            "/api/auth/login",
            new LoginCommand(
                "unknown@test.com",
                "Password123!"));

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        Context.RefreshTokens.Should().BeEmpty();
    }

    [Fact]
    public async Task Should_Return_401_When_Password_Is_Invalid()
    {
        await _authHelper.RegisterAsync();

        var response = await Client.PostAsJsonAsync(
            "/api/auth/login",
            new LoginCommand(
                "john@test.com",
                "WrongPassword"));

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        Context.RefreshTokens.Should().BeEmpty();
    }

    [Fact]
    public async Task Should_Login_With_Email_In_Different_Case()
    {
        await _authHelper.RegisterAsync();

        var response = await Client.PostAsJsonAsync(
            "/api/auth/login",
            new LoginCommand(
                "John@Test.Com",
                "Password123!"));

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Should_Trim_Email_Before_Login()
    {
        await _authHelper.RegisterAsync();

        var response = await Client.PostAsJsonAsync(
            "/api/auth/login",
            new LoginCommand(
                "   john@test.com   ",
                "Password123!"));

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Should_Create_Refresh_Token_On_Login()
    {
        await _authHelper.RegisterAsync();

        await _authHelper.LoginAsync();

        var refresh = await Context.RefreshTokens.FirstAsync();

        refresh.TokenHash.Should().NotBeNullOrWhiteSpace();
        refresh.RevokedAt.Should().BeNull();
        refresh.ExpiresAt.Should().BeAfter(DateTime.UtcNow);
    }

}