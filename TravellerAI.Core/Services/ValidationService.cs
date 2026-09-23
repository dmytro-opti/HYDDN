using System.Net.Mail;
using TravellerAI.Core.Interfaces;
using TravellerAI.Core.Repositories;
using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Models;
using static TravellerAI.Core.Constants;

namespace TravellerAI.Core.Services;

public class ValidationService : IValidationService
{
    private readonly IJourneyRepository _journeyRepository;

    public ValidationService(IJourneyRepository journeyRepository)
    {
        _journeyRepository = journeyRepository;
    }

    public Task<bool> ValidateEmail(LoginUserModel user)
    {
        var email = user.Email?.Trim();

        var isValid = !string.IsNullOrEmpty(email)
                      && email.Length <= Validation.MaxEmailLength
                      && MailAddress.TryCreate(email, out var address)
                      && address.Address == email;

        return Task.FromResult(isValid);
    }

    public Task<bool> ValidatePassword(LoginUserModel user)
    {
        var password = user.Password ?? string.Empty;

        // mirrors Identity password options configured in ServiceBootstrapper
        var isValid = password.Length is >= Security.MinPasswordLength and <= Security.MaxPasswordLength
                      && password.Any(char.IsUpper)
                      && password.Any(char.IsLower)
                      && password.Any(char.IsDigit);

        return Task.FromResult(isValid);
    }

    public Task<bool> ValidateDate(DateTime date)
    {
        return Task.FromResult(date.Date >= DateTime.UtcNow.Date);
    }

    public async Task<bool> ValidateAccessibility(Guid userId, Guid journeyId)
    {
        var journey = await _journeyRepository.GetByIdAsync(journeyId);

        return journey != null && journey.UserId == userId && journey.Status != JourneyStatus.Cancelled;
    }

    public Task<bool> ValidateBirthDate(UserInfoModel user)
    {
        if (user.BirthDate == null)
        {
            return Task.FromResult(true);
        }

        var age = UserInfoModel.CalculateAge(user.BirthDate.Value, DateTime.UtcNow);

        return Task.FromResult(age is >= Validation.MinUserAge and <= Validation.MaxUserAge);
    }

    public Task<bool> CheckString(string stringToCheck)
    {
        return Task.FromResult(!string.IsNullOrWhiteSpace(stringToCheck) && stringToCheck.Length <= Validation.MaxTextLength);
    }

    public Task<bool> CheckInt(int intToCheck)
    {
        return Task.FromResult(intToCheck >= 0);
    }

    public Task<bool> CheckPeriod(PeriodModel period)
    {
        return Task.FromResult(period != null && period.Start < period.End && period.Start.Date >= DateTime.UtcNow.Date);
    }
}
