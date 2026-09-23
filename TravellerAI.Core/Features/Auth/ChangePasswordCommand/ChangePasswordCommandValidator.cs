using FluentValidation;

namespace TravellerAI.Core.Features.Auth.ChangePasswordCommand;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId cannot be empty");
        RuleFor(x => x.CurrentPassword)
            .NotEmpty().WithMessage("CurrentPassword cannot be empty");
        RuleFor(x => x.NewPassword).StrongPassword();
        RuleFor(x => x.NewPassword)
            .NotEqual(x => x.CurrentPassword).WithMessage("New password must differ from the current one");
    }
}
