using AutoMapper;
using TravellerAI.Core.Exceptions;
using TravellerAI.Core.Interfaces;
using TravellerAI.Core.Repositories;
using TravellerAI.Domain.Entities;
using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ILoggerService<UserService> _logger;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, ILoggerService<UserService> logger, IMapper mapper)
    {
        _userRepository = userRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<UserModel> GetUserAsync(Guid userId)
    {
        var user = await GetUserEntityAsync(userId);

        return _mapper.Map<UserModel>(user);
    }

    public async Task<bool> UpdatePasswordAsync(Guid userId, string oldPassword, string newPassword)
    {
        if (oldPassword == newPassword)
        {
            throw new BadRequestException("New password must differ from the old one");
        }

        var isUpdated = await _userRepository.UpdatePasswordAsync(userId, oldPassword, newPassword);

        _logger.Log(isUpdated ? ErrorLevel.Low : ErrorLevel.Medium,
            isUpdated ? $"User {userId} password was updated" : $"User {userId} password was not updated: old password mismatch");

        return isUpdated;
    }

    public async Task UpdateNameAsync(Guid userId, string firstName, string lastName)
    {
        await _userRepository.UpdateNameAsync(userId, firstName.Trim(), lastName.Trim());
        _logger.Log(ErrorLevel.Low, $"User {userId} name was updated successfully");
    }

    public async Task UpdateEmailAsync(Guid userId, string email)
    {
        var user = await GetUserAsync(userId);

        if (string.Equals(user.Email, email, StringComparison.OrdinalIgnoreCase))
        {
            _logger.Log(ErrorLevel.Low, $"User {userId} has already been updated");
            return;
        }

        await EnsureEmailIsFreeAsync(email, userId);

        await _userRepository.UpdateEmailAsync(userId, email);
        _logger.Log(ErrorLevel.Low, $"User {userId} was updated successfully");
    }

    public async Task RemoveUserAsync(Guid userId)
    {
        await _userRepository.RemoveUserAsync(userId);
        _logger.Log(ErrorLevel.Low, $"User {userId} was removed");
    }

    public async Task<UserInfoModel> GetUserInfoAsync(Guid userId)
    {
        var user = await GetUserEntityAsync(userId);

        // lazy loaded
        if (user.UserInfo == null)
        {
            throw new NotFoundException($"User {userId} has no profile info");
        }

        return _mapper.Map<UserInfoModel>(user.UserInfo);
    }

    public async Task<bool> UpdateUserProfileAsync(UserModel user)
    {
        var entity = await GetUserEntityAsync(user.Id);

        var isEmailChanged = !string.Equals(entity.Email, user.Email, StringComparison.OrdinalIgnoreCase);
        if (isEmailChanged)
        {
            await EnsureEmailIsFreeAsync(user.Email, user.Id);
        }

        // password can be changed only via UpdatePasswordAsync which verifies the old one
        var password = entity.Password;
        _mapper.Map(user, entity);
        entity.Password = password;

        if (isEmailChanged)
        {
            entity.IsEmailConfirmed = false;
        }

        // profile preferences are stored in UserInfo, created on first profile update
        var info = entity.UserInfo ??= new UserInfoEntity();
        info.Interests = user.Interests ?? new List<string>();
        info.TravelStyle = user.TravelStyle;
        info.LookingFor = user.LookingFor;
        info.Languages = user.Languages ?? new List<string>();
        info.PersonalityType = user.PersonalityType ?? new List<string>();
        info.Age = user.Age;
        info.ChoosenActivity = user.ChoosenActivity ?? new List<string>();
        info.ChoosenTrip = user.ChoosenTrip ?? new List<string>();
        info.MoneyAmount = user.MoneyAmount ?? new List<string>();

        await _userRepository.UpdateAsync(entity);
        _logger.Log(ErrorLevel.Low, $"User {user.Id} profile was updated successfully");

        return true;
    }

    private async Task<UserEntity> GetUserEntityAsync(Guid userId)
    {
        var user = await _userRepository.GetUserAsync(userId);

        if (user == null)
        {
            _logger.Log(ErrorLevel.High, $"User {userId} not found");
            throw new NotFoundException("User", userId);
        }

        return user;
    }

    private async Task EnsureEmailIsFreeAsync(string email, Guid userId)
    {
        if (await _userRepository.IsEmailTakenAsync(email, userId))
        {
            throw new ConflictException($"Email {email} is already in use");
        }
    }
}
