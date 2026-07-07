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
        var botToken = configuration.GetValue<string>("Telegram:BotToken");

        if (string.IsNullOrWhiteSpace(botToken))
        {
            throw new InvalidOperationException("Telegram BotToken is not configured.");
        }

        services.AddSingleton<ITelegramBotClient>(provider =>
        {
            var options = provider
                .GetRequiredService<IOptions<TelegramOptions>>()
                .Value;
            return new TelegramBotClient(options.BotToken);
        });

        services.AddScoped<ITelegramHandler, StartHandler>();
        services.AddScoped<ITelegramHandler, UnknownHandler>();

        services.AddScoped<TelegramRouter>();

        return services;
    }
}