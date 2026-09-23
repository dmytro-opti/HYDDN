using FluentValidation;
using static TravellerAI.Core.Constants;
using static TravellerAI.Core.Constants.Validation;

namespace TravellerAI.Core.Features.Trips.CreateTripCommand;

public class CreateTripCommandValidator : AbstractValidator<CreateTripCommand>
{
    public CreateTripCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId cannot be empty");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty").MaximumLength(MaxTitleLength).WithMessage($"Name cannot exceed {MaxTitleLength} characters");
        RuleFor(x => x.Description).MaximumLength(MaxTextLength).WithMessage($"Description cannot exceed {MaxTextLength} characters");
        RuleFor(x => x.Stops).NotNull().Must(s => s.Count is >= Journey.MinTripStops and <= Journey.MaxTripStops)
            .WithMessage($"Trip must have {Journey.MinTripStops}-{Journey.MaxTripStops} stops");
        RuleForEach(x => x.Stops).Must(s => s.LocationId.HasValue ^ s.ActivityId.HasValue)
            .WithMessage("Every stop needs either a location or an activity");
    }
}
