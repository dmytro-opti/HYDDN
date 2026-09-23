using FluentValidation;
using static TravellerAI.Core.Constants.Validation;

namespace TravellerAI.Core.Features.GetAvailableLocationCommand;

public class GetAvailableLocationCommandValidator : AbstractValidator<GetAvailableLocationListCommand>
{
    public GetAvailableLocationCommandValidator()
    {
        RuleFor(x => x.Country)
            .NotEmpty()
            .WithMessage("Country is required")
            .MaximumLength(MaxLocationNameLength)
            .WithMessage($"Country cannot be longer than {MaxLocationNameLength} characters");

        // city is optional - all locations of the country are returned without it
        RuleFor(x => x.City)
            .MaximumLength(MaxLocationNameLength)
            .WithMessage($"City cannot be longer than {MaxLocationNameLength} characters")
            .When(x => x.City != null);
    }
}
