using FluentValidation;

namespace TravellerAI.Core.Features.Journeys.RemoveJourneyHotelCommand;

public class RemoveJourneyHotelCommandValidator : AbstractValidator<RemoveJourneyHotelCommand>
{
    public RemoveJourneyHotelCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId cannot be empty");
        RuleFor(x => x.JourneyId).NotEmpty().WithMessage("JourneyId cannot be empty");
        RuleFor(x => x.BookingId).NotEmpty().WithMessage("BookingId cannot be empty");
    }
}
