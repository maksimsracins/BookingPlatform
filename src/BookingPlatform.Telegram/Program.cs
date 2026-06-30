using BookingPlatform.Telegram;

var builder = Host.CreateApplicationBuilder(args);
builder.Services
    .AddTelegram(builder.Configuration)
    .AddHostedService<Worker>();

var host = builder.Build();
host.Run();
