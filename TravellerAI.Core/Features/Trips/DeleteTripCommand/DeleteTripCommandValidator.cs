using FluentValidation;

namespace TravellerAI.Core.Features.Trips.DeleteTripCommand;

public class DeleteTripCommandValidator : AbstractValidator<DeleteTripCommand>
{
    public DeleteTripCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId cannot be empty");
        RuleFor(x => x.TripId).NotEmpty().WithMessage("TripId cannot be empty");
    }
}
