using FluentValidation;
using static TravellerAI.Core.Constants.Security;

namespace TravellerAI.Core.Features.Auth;

public static class PasswordRules
{
    /// <summary>
    /// Password policy, the same as configured for ASP.NET Core Identity.
    /// </summary>
    public static IRuleBuilderOptions<T, string> StrongPassword<T>(this IRuleBuilder<T, string> rule)
    {
        return rule
            .NotEmpty().WithMessage("Password cannot be empty")
            .Length(MinPasswordLength, MaxPasswordLength)
            .WithMessage($"Password must be {MinPasswordLength}-{MaxPasswordLength} characters long")
            .Matches("[A-Z]").WithMessage("Password must contain an uppercase letter")
            .Matches("[a-z]").WithMessage("Password must contain a lowercase letter")
            .Matches("[0-9]").WithMessage("Password must contain a digit");
    }
}
