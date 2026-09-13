using FluentValidation;

namespace TravellerAI.Core.Features.GetAvailableLocationCommand;

public class GetAvailableLocationCommandValidator : AbstractValidator<GetAvailableLocationListCommand>
{
    public  GetAvailableLocationCommandValidator()
    {
        RuleFor(x => x.Country)
            .NotEmpty()
            .WithMessage("Country is required")
            .MaximumLength(100)
            .WithMessage("Country cannot be longer than 100 characters");
        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage("City is required")
            .MaximumLength(100)
            .WithMessage("City cannot be longer than 100 characters");
    }
}