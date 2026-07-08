using BookingPlatform.Telegram.Routing;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BookingPlatform.Telegram.Handlers;

public sealed class UnknownHandler : ITelegramHandler
{
    private readonly ITelegramBotClient _bot;

    public UnknownHandler(
        ITelegramBotClient bot)
    {
        _bot = bot;
    }

    public async Task<bool> CanHandle(Update update, CancellationToken cancellationToken)
    {
        return true;
    }

    public async Task HandleAsync(
        Update update,
        CancellationToken cancellationToken)
    {
        if (update.Message is null)
            return;

        await _bot.SendMessage(
            update.Message.Chat.Id,
            "Не понял команду 🤔",
            cancellationToken: cancellationToken);
    }
}