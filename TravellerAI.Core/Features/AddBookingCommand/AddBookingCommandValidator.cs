using FluentValidation;

namespace TravellerAI.Core.Features.AddBookingCommand;

public class AddBookingCommandValidator : AbstractValidator<AddBookingCommand>
{
    public AddBookingCommandValidator()
    {
        RuleFor(input => input.UserId)
            .NotNull().WithMessage("UserId cannot be null");
        RuleFor(input => input.PropertyId)
            .NotNull().WithMessage("PropertyId cannot be null");
        RuleFor(input => input.RoomId)
            .NotNull().WithMessage("RoomId cannot be null");
        RuleFor(input => input.JourneyId)
            .NotNull().WithMessage("JourneyId cannot be null");
        RuleFor(input => input.CheckInDate)
            .GreaterThan(DateTime.Today.AddDays(1)).WithMessage("Check in should be at least 1 day before trip");
        RuleFor(input => input.CheckOutDate)
            .GreaterThan(DateTime.Today.AddDays(1)).WithMessage("Check out should be at least 1 day before trip");
        RuleFor(input => input.GuestCount)
            .GreaterThanOrEqualTo(1).WithMessage("Guest count cannot be null");
    }
}