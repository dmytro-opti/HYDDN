using FluentValidation;

namespace TravellerAI.Core.Features.SelectTransportCommand;

public class SelectTransportCommandValidator : AbstractValidator<SelectTransportCommand>
{
    public SelectTransportCommandValidator()
    {
        RuleFor(x => x.TransportId)
            .NotNull()
            .WithMessage("Transport ID cannot be null");
    }
}