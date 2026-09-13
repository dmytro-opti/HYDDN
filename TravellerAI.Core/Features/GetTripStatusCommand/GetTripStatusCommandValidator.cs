using FluentValidation;

namespace TravellerAI.Core.Features.GetTripStatusCommand;

public class GetTripStatusCommandValidator : AbstractValidator<GetTripStatusCommand>
{
    public GetTripStatusCommandValidator()
    {
        RuleFor(x => x.TripId)
            .NotEmpty()
            .WithMessage("TripId cannot be empty");
    }
}