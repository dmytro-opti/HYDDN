using FluentValidation;

namespace TravellerAI.Core.Features.GetTripsCommand;

public class GetTripsCommandValidator : AbstractValidator<GetTripsCommand>
{
    public GetTripsCommandValidator()
    {
        RuleFor(input => input.UserId)
            .NotEmpty().WithMessage("UserId cannot be null");
    }
}