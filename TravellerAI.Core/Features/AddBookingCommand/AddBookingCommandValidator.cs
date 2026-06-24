using FluentValidation;

namespace TravellerAI.Core.Features.AddBookingCommand;

public class AddBookingCommandValidator : AbstractValidator<AddBookingCommand>
{
    public AddBookingCommandValidator()
    {
        RuleFor(input => input.UserId)
            .NotNull()
            .Must(x => Guid.TryParse(x.ToString(), out Guid _))
            .WithMessage("UserId cannot be null");
        
        RuleFor(input => input.TripId)
            .NotNull()
            .Must(x => Guid.TryParse(x.ToString(), out Guid _))
            .WithMessage("TripId cannot be null");
        
        RuleFor(input => input.Period)
            .NotNull()
            .WithMessage("Period cannot be null");
        
        RuleFor(input => input.Period)
            .NotNull()
            .Must(x => x.Start < x.End && x.Start > DateTime.Now.AddDays(1))
            .WithMessage("Selected period must be after start date and at least one day before today");
        
        RuleFor(input => input.Children)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Children must be greater than or equal 0");
        
        RuleFor(input => input.Adults)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Trip should be greater than or equal 1");
    }
}