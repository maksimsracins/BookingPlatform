using BookingPlatform.Core.Entities;

namespace BookingPlatform.IntegrationTests.Builders;

public sealed class BusinessBuilder
{
    private Guid _id = Guid.NewGuid();
    private string _name = "Test Business";
    private string _phone = "+37120000000";
    private string _address = "Riga";
    private string _telegramBotToken = "test-bot-token";
    private string _timeZone = "Europe/Riga";
    private TimeSpan _slotInterval = TimeSpan.FromMinutes(15);

    public BusinessBuilder WithSlotInterval(TimeSpan slotInterval)
    {
        _slotInterval = slotInterval;
        return this;
    }

    public BusinessBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public Business Build()
    {
        return new Business(
            _name,
            _phone,
            _address,
            _telegramBotToken,
            _timeZone,
            _slotInterval);
    }
}