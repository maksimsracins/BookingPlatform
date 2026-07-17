using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using BookingPlatform.Application.Features.Auth.Register;
using BookingPlatform.Application.Features.Authentication.GetCurrentUser;
using BookingPlatform.Application.Features.Authentication.Login;
using BookingPlatform.IntegrationTests.Infrastructure;
using FluentAssertions;

public class AuthenticationTests : IntegrationTest
{
    public AuthenticationTests(PostgreSqlFixture fixture)
        : base(fixture)
    {
    }

    [Fact]
    public async Task Should_Register_Login_And_Get_Current_User()
    {
        var register = new RegisterCommand(
            Email: "john@test.com",
            Password: "Password123!",
            BusinessName: "John's Barbershop");

        var registerResponse = await Client.PostAsJsonAsync(
            "/api/auth/register",
            register);

        registerResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var login = new LoginCommand(
            Email: register.Email,
            Password: register.Password);

        var loginResponse = await Client.PostAsJsonAsync(
            "/api/auth/login",
            login);

        loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var authentication = await loginResponse.Content
            .ReadFromJsonAsync<AuthenticationResponse>();

        authentication.Should().NotBeNull();

        authentication!.AccessToken.AccessToken.Should().NotBeNullOrWhiteSpace();

        Client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                authentication.AccessToken.AccessToken);

        var meResponse = await Client.GetAsync("/api/auth/me");

        meResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var me = await meResponse.Content
            .ReadFromJsonAsync<GetCurrentUserResponse>();

        me.Should().NotBeNull();

        me!.Email.Should().Be(register.Email.ToLowerInvariant());
    }

    [Fact]
    public async Task Should_Return_401_When_Token_Is_Missing()
    {
        var response = await Client.GetAsync("/api/auth/me");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Should_Return_401_When_Token_Is_Invalid()
    {
        Client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                "invalid-token");

        var response = await Client.GetAsync("/api/auth/me");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Should_Return_401_When_Credentials_Are_Invalid()
    {
        var command = new LoginCommand(
            "john@test.com",
            "wrong-password");

        var response = await Client.PostAsJsonAsync(
            "/api/auth/login",
            command);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Should_Return_409_When_Email_Already_Exists()
    {
        var command = new RegisterCommand(
            "john@test.com",
            "Password123!",
            "Business");

        await Client.PostAsJsonAsync(
            "/api/auth/register",
            command);

        var response = await Client.PostAsJsonAsync(
            "/api/auth/register",
            command);

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
}

public sealed record AuthenticationResponse(
    JwtToken AccessToken,
    DateTime ExpiresAt);