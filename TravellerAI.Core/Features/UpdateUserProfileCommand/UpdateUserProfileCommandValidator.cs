using FluentValidation;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.UpdateUserProfileCommand;

public class UpdateUserProfileCommandValidator : AbstractValidator<UpdateUserProfileCommand>
{
    private int MaxLenght = 100;
    public UpdateUserProfileCommandValidator()
    {
        RuleFor(input => input.UserId)
            .NotNull().WithMessage("UserId cannot be null");
        RuleFor(input => input.Name)
            .NotEmpty().WithMessage("Name cannot be empty")
            .MaximumLength(MaxLenght)
            .WithMessage($"Name cannot exceed {MaxLenght} characters");
        RuleFor(input => input.LastName)
            .NotEmpty().WithMessage("LastName cannot be empty")
            .MaximumLength(MaxLenght)
            .WithMessage($"LastName cannot exceed {MaxLenght} characters");
        RuleFor(input => input.Password)
            .NotEmpty().WithMessage("Password cannot be empty");
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
            .GreaterThan(0).WithMessage("Age must be greater than 0");
        RuleFor(input => input.ChoosenActivity)
            .NotEmpty().WithMessage("ChoosenActivity cannot be empty");
        RuleFor(input => input.ChoosenTrip)
            .NotEmpty().WithMessage("ChoosenTrip cannot be empty");
        RuleFor(input => input.MoneyAmount)
            .NotEmpty().WithMessage("MoneyAmount cannot be empty");
        RuleFor(input => input.Journeys)
            .NotEmpty().WithMessage("Journeys cannot be empty");
    }
}