using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Interfaces;

/// <summary>
/// Reusable business validation rules (in addition to FluentValidation request validators).
/// </summary>
public interface IValidationService
{
    Task<bool> ValidateEmail(LoginUserModel user);
    /// <summary>Checks the password against the identity password policy.</summary>
    Task<bool> ValidatePassword(LoginUserModel user);
    /// <summary>Date is today or in the future.</summary>
    Task<bool> ValidateDate(DateTime date);
    /// <summary>Journey exists, belongs to the user and is not cancelled.</summary>
    Task<bool> ValidateAccessibility(Guid userId, Guid journeyId);
    Task<bool> ValidateBirthDate(UserInfoModel user);
    Task<bool> CheckString(string stringToCheck);
    Task<bool> CheckInt(int intToCheck);
    Task<bool> CheckPeriod(PeriodModel period);
}
