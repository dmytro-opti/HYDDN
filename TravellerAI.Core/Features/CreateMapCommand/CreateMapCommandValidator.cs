using FluentValidation;

namespace TravellerAI.Core.Features.CreateMapCommand;

public class CreateMapCommandValidator : AbstractValidator<CreateMapCommand>
{
    public CreateMapCommandValidator()
    {
        RuleFor(x => x.Origin)
            .NotEmpty()
            .WithMessage("Origin is required");
        RuleFor(x => x.Destination)
            .NotEmpty()
            .WithMessage("Destination is required");
    }
}