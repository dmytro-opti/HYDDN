using FluentValidation;

namespace TravellerAI.Core.Features.AddTransportCommand;

public class AddTransportCommandValidator : AbstractValidator<AddTransportCommand>
{
    public AddTransportCommandValidator()
    {
        RuleFor(input => input.UserId)
            .NotEmpty().WithMessage("UserId cannot be null");
        RuleFor(input => input.JourneyId)
            .NotEmpty().WithMessage("JourneyId cannot be null");
        RuleFor(input => input.Type)
            .NotEmpty().WithMessage("Type cannot be null");
        RuleFor(input => input.Company)
            .NotEmpty().WithMessage("Company cannot be null");
        RuleFor(input => input.SeatCount).GreaterThan(0).WithMessage("SeatCount must be greater than 0");
        RuleFor(input => input.SeatClass)
            .NotEmpty().WithMessage("SeatClass cannot be null");
        
    }
}