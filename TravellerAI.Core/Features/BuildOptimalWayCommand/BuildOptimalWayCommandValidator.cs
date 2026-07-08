using FluentValidation;

namespace TravellerAI.Core.Features.BuildOptimalWayCommand;

public class BuildOptimalWayCommandValidator : AbstractValidator<BuildOptimalWayCommand>
{
    public BuildOptimalWayCommandValidator()
    {
        RuleFor(input => input.TripId)
            .NotEmpty()
            .WithMessage("TripId cannot be empty");
        RuleFor(input => input.EndPoint)
            .NotNull()
            .WithMessage("Endpoint is required.");
        RuleFor(input => input.StartPoint)
            .NotNull()
            .WithMessage("Start point is required.");
        RuleFor(input => input.OptimizationType)
            .IsInEnum()
            .WithMessage("Invalid route optimization type selected.");
        RuleFor(input => input.TravelMode)
            .IsInEnum()
            .WithMessage("Invalid travel mode selected.");
        RuleFor(input => input.Waypoints)
            .NotNull()
            .WithMessage("Waypoints cannot be null");
            
    }
}