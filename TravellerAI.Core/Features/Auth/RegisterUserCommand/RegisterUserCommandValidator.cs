using FluentValidation;
using TravellerAI.Core.Features.Auth;
using static TravellerAI.Core.Constants.Validation;

namespace TravellerAI.Core.Features.Auth.RegisterUserCommand;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email cannot be empty")
            .MaximumLength(MaxEmailLength).WithMessage($"Email cannot exceed {MaxEmailLength} characters")
            .EmailAddress().WithMessage("Invalid email format");
        RuleFor(x => x.Password).StrongPassword();
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name cannot be empty")
            .MaximumLength(MaxNameLength).WithMessage($"Name cannot exceed {MaxNameLength} characters");
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("FirstName cannot be empty")
            .MaximumLength(MaxNameLength).WithMessage($"FirstName cannot exceed {MaxNameLength} characters");
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("LastName cannot be empty")
            .MaximumLength(MaxNameLength).WithMessage($"LastName cannot exceed {MaxNameLength} characters");
    }
}
