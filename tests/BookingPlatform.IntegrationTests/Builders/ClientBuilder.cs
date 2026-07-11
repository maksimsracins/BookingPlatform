using BookingPlatform.Core.Entities;

namespace BookingPlatform.IntegrationTests.Builders;

public sealed class ClientBuilder
{
    private Guid _id = Guid.NewGuid();
    private Guid _businessId;

    private string _fullName = "Test Client";
    private string _phone = "+37120000000";
    private string _externalId = "123456789";
    private string _username = "test_user";
    private string _notes = "";

    public ClientBuilder ForBusiness(Guid businessId)
    {
        _businessId = businessId;
        return this;
    }

    public Client Build()
    {
        return Client.Create(
            _businessId,    
            _fullName,
            _phone,
            _externalId);
    }
}