using FluentValidation;
using FluentValidation.Validators;

namespace TravellerAI.Core.Features.UpdateTripCommand;

public class UpdateTripCommandValidator : AbstractValidator<UpdateTripCommand>
{
    private int MaxLenght = 100;
    private int MinRating = 0;
    private int MaxRating = 10;
    public UpdateTripCommandValidator()
    {
        RuleFor(input => input.TripId)
            .NotEmpty().WithMessage("TripId cannot be null");
        RuleFor(input => input.Name)
            .NotEmpty().WithMessage("Name cannot be null")
            .MaximumLength(MaxLenght)
            .WithMessage($"Name cannot exceed {MaxLenght} characters");
        RuleFor(input => input.Group)
            .NotEmpty().WithMessage("Group cannot be null");
        RuleFor(input => input.Booking)
            .NotEmpty().WithMessage("Booking cannot be null");
        RuleFor(input => input.Period)
            .NotEmpty().WithMessage("Period cannot be null")
            .Must(period => period == null || period.Start < period.End)
            .WithMessage("Period start date must be before end date");
        RuleFor(input => input.Rating)
            .NotEmpty().WithMessage("Rating cannot be null")
            .InclusiveBetween(MinRating, MaxRating)
            .WithMessage($"Rating must be between {MinRating} and {MaxRating}");
    }
}