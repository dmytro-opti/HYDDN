using FluentValidation;
using static TravellerAI.Core.Constants.Validation;

namespace TravellerAI.Core.Features.UpdateTripCommand;

public class UpdateTripCommandValidator : AbstractValidator<UpdateTripCommand>
{
    public UpdateTripCommandValidator()
    {
        RuleFor(input => input.TripId)
            .NotEmpty().WithMessage("TripId cannot be empty");
        RuleFor(input => input.Name)
            .NotEmpty().WithMessage("Name cannot be empty")
            .MaximumLength(MaxNameLength)
            .WithMessage($"Name cannot exceed {MaxNameLength} characters");
        RuleFor(input => input.Period)
            .NotNull().WithMessage("Period cannot be null")
            .Must(period => period == null || period.Start < period.End)
            .WithMessage("Period start date must be before end date");
        RuleFor(input => input.Rating)
            .InclusiveBetween(MinRating, MaxRating)
            .WithMessage($"Rating must be between {MinRating} and {MaxRating}");
    }
}
