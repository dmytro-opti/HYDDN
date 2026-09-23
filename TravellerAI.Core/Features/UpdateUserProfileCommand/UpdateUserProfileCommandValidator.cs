using FluentValidation;
using static TravellerAI.Core.Constants.Validation;

namespace TravellerAI.Core.Features.UpdateUserProfileCommand;

public class UpdateUserProfileCommandValidator : AbstractValidator<UpdateUserProfileCommand>
{
    public UpdateUserProfileCommandValidator()
    {
        RuleFor(input => input.UserId)
            .NotEmpty().WithMessage("UserId cannot be empty");
        RuleFor(input => input.Name)
            .NotEmpty().WithMessage("Name cannot be empty")
            .MaximumLength(MaxNameLength)
            .WithMessage($"Name cannot exceed {MaxNameLength} characters");
        RuleFor(input => input.LastName)
            .NotEmpty().WithMessage("LastName cannot be empty")
            .MaximumLength(MaxNameLength)
            .WithMessage($"LastName cannot exceed {MaxNameLength} characters");
        RuleFor(input => input.Email)
            .NotEmpty().WithMessage("Email cannot be empty")
            .EmailAddress().WithMessage("Invalid email format");
        RuleFor(input => input.Interests)
            .NotNull().WithMessage("Interests cannot be null");
        RuleFor(input => input.TravelStyle)
            .NotEmpty().WithMessage("TravelStyle cannot be empty");
        RuleFor(input => input.LookingFor)
            .NotEmpty().WithMessage("Looking for cannot be empty");
        RuleFor(input => input.Languages)
            .NotEmpty().WithMessage("Languages cannot be empty");
        RuleFor(input => input.PersonalityType)
            .NotEmpty().WithMessage("Personality type cannot be empty");
        RuleFor(input => input.Age)
            .GreaterThanOrEqualTo(MinAge).WithMessage($"Age must be at least {MinAge}");
        RuleFor(input => input.ChoosenActivity)
            .NotEmpty().WithMessage("ChoosenActivity cannot be empty");
        RuleFor(input => input.ChoosenTrip)
            .NotEmpty().WithMessage("ChoosenTrip cannot be empty");
        RuleFor(input => input.MoneyAmount)
            .NotEmpty().WithMessage("MoneyAmount cannot be empty");
    }
}
