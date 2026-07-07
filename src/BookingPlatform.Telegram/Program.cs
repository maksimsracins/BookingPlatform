using BookingPlatform.Application;
using BookingPlatform.Infrastructure;
using BookingPlatform.Telegram;

var builder = Host.CreateApplicationBuilder(args);
builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddTelegram(builder.Configuration)
    .AddHostedService<TelegramWorker>();

var host = builder.Build();

host.Run();
