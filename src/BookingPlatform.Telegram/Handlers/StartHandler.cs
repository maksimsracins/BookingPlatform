using System.Runtime.CompilerServices;
using BookingPlatform.Telegram.Routing;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

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

        var keyboard = new InlineKeyboardMarkup(
            [
                [
                    InlineKeyboardButton.WithCallbackData(
                        "📅 Записаться",
                        "booking_start")
                ],
                [
                    InlineKeyboardButton.WithCallbackData(
                        "📖 Мои записи",
                        "my_bookings")
                ]
            ]
        );

        await _bot.SendMessage(
            chatId: update.Message!.Chat.Id,
            text: """
                  Добро пожаловать 👋

                  Выберите действие.
                  """,
            replyMarkup: keyboard,
            cancellationToken: cancellationToken);
    }
}