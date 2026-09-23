using FluentValidation;
using static TravellerAI.Core.Constants.Validation;

namespace TravellerAI.Core.Features.UpdateBookingCommand;

public class UpdateBookingCommandValidator : AbstractValidator<UpdateBookingCommand>
{
    public UpdateBookingCommandValidator()
    {
        RuleFor(input => input.UserId)
            .NotEmpty()
            .WithMessage("UserId cannot be empty");
        
        RuleFor(input => input.BookingId)
            .NotEmpty()
            .WithMessage("BookingId cannot be empty");
        
        RuleFor(input => input.Period)
            .NotNull()
            .WithMessage("Period cannot be null");
        
        RuleFor(input => input.Period)
            .Must(x => x.Start < x.End && x.Start > DateTime.UtcNow.AddDays(MinDaysBeforeBooking))
            .When(input => input.Period != null)
            .WithMessage($"Period start must be before its end and at least {MinDaysBeforeBooking} day(s) from now");
        
        RuleFor(input => input.Children)
            .GreaterThanOrEqualTo(MinChildren)
            .WithMessage($"Children must be greater than or equal {MinChildren}");
        
        RuleFor(input => input.Adults)
            .GreaterThanOrEqualTo(MinAdults)
            .WithMessage($"Adults must be greater than or equal {MinAdults}");
    }
}
