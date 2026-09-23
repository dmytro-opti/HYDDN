using FluentValidation;
using static TravellerAI.Core.Constants;
using static TravellerAI.Core.Constants.Validation;

namespace TravellerAI.Core.Features.Journeys.AddJourneyHotelCommand;

public class AddJourneyHotelCommandValidator : AbstractValidator<AddJourneyHotelCommand>
{
    public AddJourneyHotelCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId cannot be empty");
        RuleFor(x => x.JourneyId).NotEmpty().WithMessage("JourneyId cannot be empty");
        RuleFor(x => x.PlaceId).NotEmpty().WithMessage("PlaceId cannot be empty");
        RuleFor(x => x.CheckOut.Date).GreaterThan(x => x.CheckIn.Date).WithMessage("Check-out has to be after check-in");
        RuleFor(x => x.Adults).GreaterThanOrEqualTo(MinAdults).WithMessage($"Adults must be at least {MinAdults}");
        RuleFor(x => x.Children).GreaterThanOrEqualTo(MinChildren).WithMessage($"Children must be at least {MinChildren}");
    }
}
