using FluentValidation;

namespace TravellerAI.Core.Features.Journeys.GetDayTripsCommand;

public class GetDayTripsCommandValidator : AbstractValidator<GetDayTripsCommand>
{
    public GetDayTripsCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId cannot be empty");
        RuleFor(x => x.JourneyId).NotEmpty().WithMessage("JourneyId cannot be empty");
    }
}
