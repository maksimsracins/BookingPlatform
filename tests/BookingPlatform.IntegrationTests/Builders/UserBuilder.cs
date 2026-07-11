using BookingPlatform.Core.Entities;

namespace BookingPlatform.IntegrationTests.Builders;

public sealed class UserBuilder
{
    private Guid _id = Guid.NewGuid();
    private string _email = "test-email@test.com";
    private string _passwordHash = "test-password-hash";


    public UserBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public UserBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public UserBuilder WithPasswordHash(string passwordHash)
    {
        _passwordHash = passwordHash;
        return this;
    }

    public User Build()
    {
        return User.Create(
            _email,
            _passwordHash);
    }
}