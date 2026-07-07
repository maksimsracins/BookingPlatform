using Telegram.Bot.Types;

namespace BookingPlatform.Telegram.Routing;

public sealed class TelegramRouter
{
    private readonly IEnumerable<ITelegramHandler> _handlers;

    public TelegramRouter(
        IEnumerable<ITelegramHandler> handlers)
    {
        _handlers = handlers;
    }

    public async Task HandleAsync(Update update, CancellationToken cancellationToken)
    {
        ITelegramHandler? handler = null;

        foreach (var candidate in _handlers)
        {
            if (await candidate.CanHandle(update, cancellationToken))
            {
                handler = candidate;
                break;
            }
        }

        if (handler is null)
        {
            return;
        }

        await handler.HandleAsync(update, cancellationToken);
    }
}