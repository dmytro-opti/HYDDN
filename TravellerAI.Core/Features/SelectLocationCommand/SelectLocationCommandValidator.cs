using FluentValidation;

namespace TravellerAI.Core.Features.SelectLocationCommand;

public class SelectLocationCommandValidator : AbstractValidator<SelectLocationCommand>
{
    public SelectLocationCommandValidator()
    {
        RuleFor(x => x.LocationId)
            .NotEmpty()
            .WithMessage("LocationId cannot be empty");
    }
}