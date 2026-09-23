using FluentValidation;

namespace TravellerAI.Core.Features.SelectBookingCommand;

public class SelectBookingCommandValidator : AbstractValidator<SelectBookingCommand>
{
    public SelectBookingCommandValidator()
    {
        RuleFor(input => input.UserId)
            .NotEmpty()
            .WithMessage("UserId cannot be empty");
        RuleFor(input => input.BookingId)
            .NotEmpty()
            .WithMessage("BookingId cannot be empty");
        RuleFor(input => input.TripId)
            .NotEmpty()
            .WithMessage("TripId cannot be empty");
    }
}
