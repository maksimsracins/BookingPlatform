using Telegram.Bot.Types;

namespace BookingPlatform.Telegram.Routing;

public interface ITelegramHandler
{
    Task<bool> CanHandle(Update update, CancellationToken cancellationToken);

    Task HandleAsync(Update update, CancellationToken cancellationToken);
}