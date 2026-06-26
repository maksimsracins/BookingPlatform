using BookingPlatform.Core.Entities;

namespace BookingPlatform.IntegrationTests.Builders;

public sealed class EmployeeBuilder
{
    private Guid _id = Guid.NewGuid();
    private Guid _businessId = Guid.NewGuid();

    private string _fullName = "John Smith";
    private string _phone = "+37120000000";
    private string _color = "#2196F3";
    private bool _isActive = true;

    public EmployeeBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public EmployeeBuilder ForBusiness(Guid businessId)
    {
        _businessId = businessId;
        return this;
    }

    public EmployeeBuilder WithName(string fullName)
    {
        _fullName = fullName;
        return this;
    }

    public EmployeeBuilder WithPhone(string phone)
    {
        _phone = phone;
        return this;
    }

    public EmployeeBuilder WithColor(string color)
    {
        _color = color;
        return this;
    }

    public EmployeeBuilder Inactive()
    {
        _isActive = false;
        return this;
    }

    public Employee Build()
    {
        var employee = new Employee(
            _id,
            _businessId,
            _fullName,
            _phone,
            _color);

        if (!_isActive)
        {
            employee.Deactivate();
        }
        
        return employee;
    }
}