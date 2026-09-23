using FluentValidation;
using static TravellerAI.Core.Constants;
using static TravellerAI.Core.Constants.Validation;

namespace TravellerAI.Core.Features.Journeys.UpdateJourneyHotelCommand;

public class UpdateJourneyHotelCommandValidator : AbstractValidator<UpdateJourneyHotelCommand>
{
    public UpdateJourneyHotelCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId cannot be empty");
        RuleFor(x => x.JourneyId).NotEmpty().WithMessage("JourneyId cannot be empty");
        RuleFor(x => x.BookingId).NotEmpty().WithMessage("BookingId cannot be empty");
        RuleFor(x => x.CheckOut.Date).GreaterThan(x => x.CheckIn.Date).WithMessage("Check-out has to be after check-in");
        RuleFor(x => x.Adults).GreaterThanOrEqualTo(MinAdults).WithMessage($"Adults must be at least {MinAdults}");
        RuleFor(x => x.Children).GreaterThanOrEqualTo(MinChildren).WithMessage($"Children must be at least {MinChildren}");
    }
}
