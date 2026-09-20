using FluentValidation;

namespace TravellerAI.Core.Features.GetAvailableLocationCommand;

public class GetAvailableLocationCommandValidator : AbstractValidator<GetAvailableLocationListCommand>
{
    public int maxStringLenght = 100;
    public  GetAvailableLocationCommandValidator()
    {
        RuleFor(x => x.Country)
            .NotEmpty()
            .WithMessage("Country is required")
            .MaximumLength(maxStringLenght)
            .WithMessage($"Country cannot be longer than {maxStringLenght} characters");
        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage("City is required")
            .MaximumLength(maxStringLenght)
            .WithMessage($"City cannot be longer than {maxStringLenght} characters");
    }
}