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
            .MaximumLength(MaxNameLength).WithMessage($"Name cannot exceed {MaxNameLength} characters");
        RuleFor(input => input.FirstName)
            .NotEmpty().WithMessage("FirstName cannot be empty")
            .MaximumLength(MaxNameLength).WithMessage($"FirstName cannot exceed {MaxNameLength} characters");
        RuleFor(input => input.LastName)
            .NotEmpty().WithMessage("LastName cannot be empty")
            .MaximumLength(MaxNameLength).WithMessage($"LastName cannot exceed {MaxNameLength} characters");
        RuleFor(input => input.BirthDate)
            .Must(date => date!.Value.Date <= DateTime.UtcNow.Date.AddYears(-MinUserAge)
                          && date.Value.Date >= DateTime.UtcNow.Date.AddYears(-MaxUserAge))
            .When(input => input.BirthDate.HasValue)
            .WithMessage($"Age must be between {MinUserAge} and {MaxUserAge} years");
        RuleFor(input => input.TravelStyle).IsInEnum().WithMessage("Invalid travel style");
        RuleFor(input => input.PersonalityType).IsInEnum().WithMessage("Invalid personality type");
        RuleFor(input => input.BudgetLevel).IsInEnum().WithMessage("Invalid budget level");
        RuleFor(input => input.CompanionGender).IsInEnum().WithMessage("Invalid companion gender");
        RuleFor(input => input.LookingFor)
            .MaximumLength(MaxTextLength).WithMessage($"LookingFor cannot exceed {MaxTextLength} characters");
        RuleFor(input => input.Languages)
            .NotNull().WithMessage("Languages cannot be null");
        RuleForEach(input => input.Languages)
            .Matches($"^[a-zA-Z]{{{LanguageCodeLength}}}$")
            .WithMessage($"Language must be an ISO 639-1 code ({LanguageCodeLength} letters)");
        RuleFor(input => input.Interests)
            .NotNull().WithMessage("Interests cannot be null");
        RuleForEach(input => input.Interests).IsInEnum().WithMessage("Invalid interest");
        RuleFor(input => input.ChosenActivityIds)
            .Must(ids => ids.Count <= MaxProfileSelections)
            .WithMessage($"Up to {MaxProfileSelections} activities can be chosen");
        RuleFor(input => input.ChosenTripIds)
            .Must(ids => ids.Count <= MaxProfileSelections)
            .WithMessage($"Up to {MaxProfileSelections} trips can be chosen");
        RuleFor(input => input.PreferredCountryIds)
            .Must(ids => ids.Count <= MaxProfileSelections)
            .WithMessage($"Up to {MaxProfileSelections} countries can be chosen");
    }
}
