using FluentValidation;

namespace TravellerAI.Core.Features.User.DeleteAccountCommand;

public class DeleteAccountCommandValidator : AbstractValidator<DeleteAccountCommand>
{
    public DeleteAccountCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId cannot be empty");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password cannot be empty");
    }
}
