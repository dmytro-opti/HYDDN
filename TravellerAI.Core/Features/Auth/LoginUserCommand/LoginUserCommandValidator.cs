using FluentValidation;
using static TravellerAI.Core.Constants;

namespace TravellerAI.Core.Features.Auth.LoginUserCommand;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email cannot be empty")
            .MaximumLength(Validation.MaxEmailLength).WithMessage($"Email cannot exceed {Validation.MaxEmailLength} characters");
        // password policy is not revealed on login
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password cannot be empty")
            .MaximumLength(Security.MaxPasswordLength).WithMessage($"Password cannot exceed {Security.MaxPasswordLength} characters");
    }
}
