namespace BookingPlatform.Core.Entities;

public class Client
{
    private Client() { }
    private Client(Guid businessId, string fullName, string phone, string externalId, string notes = "")
    {
        Id = Guid.NewGuid();
        BusinessId = businessId;
        FullName = fullName;
        Phone = phone;
        ExternalId = externalId;
        Notes = notes;
    }

    public static Client Create(
        Guid businessId,
        string name,
        string phone,
        string externalId)
    {
        return new Client(
            businessId,
            name,
            phone,
            externalId);
    }

    public Guid Id { get; private set; }
    public Guid BusinessId { get; private set; }
    public Business Business { get; private set; } = null!;
    public string FullName { get; private set; } = string.Empty;
    public string ExternalId { get; private set; } = string.Empty;
    public string Notes { get; private set; } = string.Empty;
    public string Phone { get; private set; } = string.Empty;

    public ICollection<Appointment> Appointments { get; private set; } = [];

    public void ChangePhone(string phone)
    {
        Phone = phone;
    }

}