using System.Net;
using System.Net.Http.Json;
using BookingPlatform.Application.Common.Authentication;
using BookingPlatform.Application.Features.Auth.Register;
using BookingPlatform.Application.Features.Authentication.Login;
using BookingPlatform.IntegrationTests.Infrastructure;
using FluentAssertions;

namespace BookingPlatform.IntegrationTests.Auth;

public sealed class AuthenticationTestHelper
{
    private readonly HttpClient _client = null!;

    public AuthenticationTestHelper(HttpClient client)
    {
        _client = client;
    }
    public async Task<AuthenticationResult> LoginAsync(string email = "john@test.com", string password = "Password123!")
    {
        var response = await _client.PostAsJsonAsync(
            "/api/auth/login",
            new LoginCommand(
                email,
                password));

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        return (await response.Content.ReadFromJsonAsync<AuthenticationResult>())!;
    }

    public async Task RegisterAsync(string email = "john@test.com", string password = "Password123!", string businessName = "MyBusinessName")
    {
        var response = await _client.PostAsJsonAsync(
            "/api/auth/register",
            new RegisterCommand(
                email,
                password,
                businessName));

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    public async Task<string> GetAccessTokenAsync(string email = "john@test.com", string password = "Password123!")
    {
        var response = await _client.PostAsJsonAsync(
            "/api/auth/login",
            new LoginCommand(email, password));

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

        return result!.AccessToken;
    }
}