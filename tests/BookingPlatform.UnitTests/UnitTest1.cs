using BookingPlatform.Api.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookingPlatform.Tests;

public class JwtAuthenticationTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("short")]
    [InlineData("1234567890123456789012345678901")]
    public void AddJwtAuthentication_RejectsMissingOrShortSigningKeys(string? secretKey)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:SecretKey"] = secretKey
            })
            .Build();

        var services = new ServiceCollection();

        var exception = Assert.Throws<InvalidOperationException>(
            () => services.AddJwtAuthentication(configuration));

        Assert.Contains("at least 32 bytes", exception.Message);
    }

    [Fact]
    public void AddJwtAuthentication_AcceptsSigningKeyOfAtLeast32Bytes()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:SecretKey"] = "12345678901234567890123456789012"
            })
            .Build();

        var services = new ServiceCollection();

        services.AddJwtAuthentication(configuration);

        Assert.NotEmpty(services);
    }
}
