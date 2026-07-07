using System.Security.Cryptography.X509Certificates;
using BookingPlatform.Application.Features.Business.GetBusinesses;
using BookingPlatform.Core.Entities;
using BookingPlatform.Telegram.Routing;
using MediatR;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace BookingPlatform.Telegram.Handlers;

public sealed class BookingStartHandler : ITelegramHandler
{
    private readonly ITelegramBotClient _bot;
    private readonly IMediator _mediator;

    public BookingStartHandler(ITelegramBotClient bot, IMediator mediator)
    {
        _bot = bot;
        _mediator = mediator;
    }

    public Task<bool> CanHandle(Update update, CancellationToken cancellationToken)
    {
        return Task.FromResult(update.CallbackQuery?.Data == "booking_start");
    }

    public async Task HandleAsync(Update update, CancellationToken cancellationToken)
    {
        var callback = update.CallbackQuery!;

        await _bot.AnswerCallbackQuery(
            callback.Id,
            cancellationToken: cancellationToken);

        await _bot.SendMessage(
            callback.Message!.Chat.Id,
            "Получаем список компаний...",
            cancellationToken: cancellationToken);

        var businesses = await _mediator.Send(new GetBusinessesQuery(), cancellationToken);
        var keyboard = new InlineKeyboardMarkup(
            businesses.Select(x => new[]
            {
                InlineKeyboardButton.WithCallbackData(
                    x.Name, $"business:{x.Id}")
            })
        );

            await _bot.SendMessage(callback.Message!.Chat.Id, "Выберите компанию", replyMarkup: keyboard,cancellationToken: cancellationToken);
    }
}