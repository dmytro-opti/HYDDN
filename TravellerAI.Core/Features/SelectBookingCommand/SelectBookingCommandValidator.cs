using FluentValidation;

namespace TravellerAI.Core.Features.SelectBookingCommand;

public class SelectBookingCommandValidator : AbstractValidator<SelectBookingCommand>
{
    public SelectBookingCommandValidator()
    {
        RuleFor(input => input.UserId)
            .NotNull()
            .Must(x => Guid.TryParse(x.ToString(), out Guid _))
            .WithMessage("UserId cannot be null");
        RuleFor(input => input.BookingId)
            .NotNull()
            .Must(x => Guid.TryParse(x.ToString(), out Guid _))
            .WithMessage("BookingId cannot be null");
        RuleFor(input => input.TripId)
            .NotNull()
            .Must(x => Guid.TryParse(x.ToString(), out Guid _))
            .WithMessage("TripId cannot be null");
    }
}