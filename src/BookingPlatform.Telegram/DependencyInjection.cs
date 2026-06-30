using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

namespace BookingPlatform.Telegram;

public static class DependencyInjection
{
    public static IServiceCollection AddTelegram(this IServiceCollection services, IConfiguration configuration)
    {
        var botToken = configuration.GetValue<string>("Telegram:BotToken");

        if (string.IsNullOrWhiteSpace(botToken))
        {
            throw new InvalidOperationException("Telegram BotToken is not configured.");
        }

        services.AddSingleton<ITelegramBotClient>(_ =>
            new TelegramBotClient(botToken));

        return services;
    }
}