using FluentValidation;

namespace BookingPlatform.Application.Features.Booking.Create;

public sealed class CreateBookingValidator : AbstractValidator<CreateBookingCommand>
{
    public CreateBookingValidator()
    {
        RuleFor(x => x.BusinessId)
            .NotEmpty();

        RuleFor(x => x.ClientId)
            .NotEmpty();

        RuleFor(x => x.EmployeeId)
            .NotEmpty();

        RuleFor(x => x.ServiceId)
            .NotEmpty();

        RuleFor(x => x.StartAt)
            .GreaterThan(DateTime.UtcNow);
    }
}