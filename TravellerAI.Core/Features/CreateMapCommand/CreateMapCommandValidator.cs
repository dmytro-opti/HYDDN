using FluentValidation;

namespace TravellerAI.Core.Features.CreateMapCommand;

public class CreateMapCommandValidator : AbstractValidator<CreateMapCommand>
{
    public CreateMapCommandValidator()
    {
        RuleFor(x => x.Origin)
            .NotNull()
            .WithMessage("Origin cannot be null");
        RuleFor(x => x.Destination)
            .NotNull()
            .WithMessage("Destination cannot be null");
    }
}