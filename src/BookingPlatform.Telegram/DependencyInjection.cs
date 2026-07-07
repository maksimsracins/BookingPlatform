using BookingPlatform.Core.Entities;
using BookingPlatform.Telegram.Handlers;
using BookingPlatform.Telegram.Options;
using BookingPlatform.Telegram.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace BookingPlatform.Telegram;

public static class DependencyInjection
{
    public static IServiceCollection AddTelegram(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<TelegramOptions>()
            .Bind(configuration.GetSection(TelegramOptions.SectionName));

        var botToken = configuration["Telegram:BotToken"]
            ?? configuration["Telegram__BotToken"]
            ?? Environment.GetEnvironmentVariable("Telegram__BotToken");

        if (string.IsNullOrWhiteSpace(botToken))
        {
            throw new InvalidOperationException(
                "Telegram BotToken is not configured. Set it in user secrets, environment variable 'Telegram__BotToken', or appsettings.");
        }

        services.AddSingleton<ITelegramBotClient>(_ => new TelegramBotClient(botToken));

        services.AddSingleton<ITelegramHandler, StartHandler>();
        services.AddSingleton<ITelegramHandler, UnknownHandler>();

        services.AddSingleton<TelegramRouter>();

        return services;
    }
}