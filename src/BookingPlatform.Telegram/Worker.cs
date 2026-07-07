using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using BookingPlatform.Telegram.Routing;

namespace BookingPlatform.Telegram;

public sealed class TelegramWorker : BackgroundService
{
    private readonly ITelegramBotClient _bot;
    private readonly TelegramRouter _router;
    private readonly ILogger<TelegramWorker> _logger;

    public TelegramWorker(
        ITelegramBotClient bot,
        TelegramRouter router,
        ILogger<TelegramWorker> logger)
    {
        _bot = bot;
        _router = router;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>()
        };

        _bot.StartReceiving(
            updateHandler: HandleUpdateAsync,
            errorHandler: HandleErrorAsync,
            receiverOptions: receiverOptions,
            cancellationToken: stoppingToken);

        _logger.LogInformation("Telegram bot started");

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private async Task HandleUpdateAsync(
        ITelegramBotClient bot,
        Update update,
        CancellationToken cancellationToken)
    {
        try
        {
            await _router.HandleAsync(update, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling update");
        }
    }

    private Task HandleErrorAsync(
        ITelegramBotClient bot,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Telegram polling error");
        return Task.CompletedTask;
    }
}