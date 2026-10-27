using System.Net;
using System.Net.Http.Json;
using BookingPlatform.Application.Features.Authentication.Login;
using BookingPlatform.Application.Features.Authentication.Refresh;
using BookingPlatform.Application.Features.Auth.Register;
using BookingPlatform.Core.Entities;
using BookingPlatform.Infrastructure.Authentication;
using BookingPlatform.IntegrationTests.Infrastructure;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace BookingPlatform.IntegrationTests.Auth;

public sealed class LoginRefreshTests : IntegrationTest
{
    private const string Password = "TestPassword123!";

    public LoginRefreshTests(PostgreSqlFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task Should_Return_Access_And_Refresh_Tokens_When_Login_Succeeds()
    {
        var email = await RegisterUserAsync();

        var response = await LoginAsync(email, Password);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tokens = await response.Content.ReadFromJsonAsync<LoginResponse>();
        tokens!.AccessToken.Should().NotBeNullOrWhiteSpace();
        tokens.RefreshToken.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Should_Return_401_When_Login_Password_Is_Wrong()
    {
        var email = await RegisterUserAsync();

        var response = await LoginAsync(email, "WrongPassword!");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Should_Rotate_Refresh_Token_When_Refresh_Succeeds()
    {
        var email = await RegisterUserAsync();
        var login = await ReadLoginAsync(email);

        var refresh = await RefreshAsync(login.RefreshToken);

        refresh.StatusCode.Should().Be(HttpStatusCode.OK);

        var tokens = await refresh.Content.ReadFromJsonAsync<RefreshResponse>();
        tokens!.AccessToken.Should().NotBeNullOrWhiteSpace();
        tokens.RefreshToken.Should().NotBe(login.RefreshToken);
    }

    [Fact]
    public async Task Should_Return_401_When_Rotated_Refresh_Token_Is_Reused()
    {
        var email = await RegisterUserAsync();
        var login = await ReadLoginAsync(email);

        await RefreshAsync(login.RefreshToken);
        var reuse = await RefreshAsync(login.RefreshToken);

        reuse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Should_Revoke_All_Sessions_When_Revoked_Refresh_Token_Is_Reused()
    {
        var email = await RegisterUserAsync();
        var login = await ReadLoginAsync(email);
        var rotated = await ReadRefreshAsync(login.RefreshToken);

        await RefreshAsync(login.RefreshToken);

        var activeSession = await RefreshAsync(rotated.RefreshToken);

        activeSession.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Should_Return_401_When_Refresh_Token_Is_Unknown()
    {
        var response = await RefreshAsync("unknown-refresh-token");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Should_Return_401_When_Refresh_Token_Is_Expired()
    {
        var email = await RegisterUserAsync();
        var user = await Context.Users.SingleAsync(x => x.Email == email);
        const string rawToken = "expired-refresh-token";

        Context.RefreshTokens.Add(
            new RefreshToken(
                Guid.NewGuid(),
                user.Id,
                new TokenHasher().Hash(rawToken),
                DateTime.UtcNow.AddMinutes(-1),
                deviceName: null,
                ipAddress: null));
        await Context.SaveChangesAsync();

        var response = await RefreshAsync(rawToken);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    private async Task<string> RegisterUserAsync()
    {
        var email = Guid.NewGuid() + "@test.com";
        var businessName = "Test Business " + Guid.NewGuid();

        var response = await Client.PostAsJsonAsync(
            "/api/auth/register",
            new RegisterCommand(email, Password, businessName));

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        return email;
    }

    private Task<HttpResponseMessage> LoginAsync(string email, string password)
    {
        return Client.PostAsJsonAsync(
            "/api/auth/login",
            new LoginCommand(email, password));
    }

    private Task<HttpResponseMessage> RefreshAsync(string refreshToken)
    {
        return Client.PostAsJsonAsync(
            "/api/auth/refresh",
            new RefreshCommand(refreshToken));
    }

    private async Task<LoginResponse> ReadLoginAsync(string email)
    {
        var response = await LoginAsync(email, Password);

        return (await response.Content.ReadFromJsonAsync<LoginResponse>())!;
    }

    private async Task<RefreshResponse> ReadRefreshAsync(string refreshToken)
    {
        var response = await RefreshAsync(refreshToken);

        return (await response.Content.ReadFromJsonAsync<RefreshResponse>())!;
    }
}
