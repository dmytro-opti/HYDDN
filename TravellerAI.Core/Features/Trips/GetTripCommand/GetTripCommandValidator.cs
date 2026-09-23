using FluentValidation;

namespace TravellerAI.Core.Features.Trips.GetTripCommand;

public class GetTripCommandValidator : AbstractValidator<GetTripCommand>
{
    public GetTripCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId cannot be empty");
        RuleFor(x => x.TripId).NotEmpty().WithMessage("TripId cannot be empty");
    }
}
