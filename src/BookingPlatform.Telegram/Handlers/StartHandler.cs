using BookingPlatform.Telegram.Routing;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BookingPlatform.Telegram.Handlers;

public sealed class StartHandler : ITelegramHandler
{
    private readonly ITelegramBotClient _bot;

    public StartHandler(ITelegramBotClient bot)
    {
        _bot = bot;
    }

    public Task<bool> CanHandle(Update update, CancellationToken cancellationToken)
    {
        return Task.FromResult(update.Message?.Text == "/start");
    }

    public async Task HandleAsync(Update update, CancellationToken cancellationToken)
    {
        await _bot.SendMessage(
            chatId: update.Message!.Chat.Id,
            text: """
                  Добро пожаловать 👋

                  Выберите действие.
                  """,
            cancellationToken: cancellationToken);
    }
}