using AutoMapper;
using TravellerAI.Core.Exceptions;
using TravellerAI.Core.Interfaces;
using TravellerAI.Core.Repositories;
using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Exceptions;
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
        var user = await _userRepository.GetUserAsync(userId);

        if (user == null)
        {
            _logger.Log(ErrorLevel.High, $"User {userId} not found");
            throw new ResourceNotFoundException($"User {userId} not found");
        }

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

        if (user.Email == email)
        {
            _logger.Log(ErrorLevel.Low, $"User {userId} has already been updated");
            return;
        }

        if (await _userRepository.IsEmailTakenAsync(email, userId))
        {
            throw new BadRequestException($"Email {email} is already in use");
        }

        await _userRepository.UpdateEmailAsync(userId, email);
        _logger.Log(ErrorLevel.Low, $"User {userId} was updated successfully");
    }

    public async Task RemoveUserAsync(Guid userId)
    {
        await _userRepository.RemoveUserAsync(userId);
        _logger.Log(ErrorLevel.Low, $"User {userId} was removed");
    }

    public async Task<UserInfoModel?> GetUserInfoAsync(Guid userId)
    {
        var user = await _userRepository.GetUserAsync(userId)
                   ?? throw new ResourceNotFoundException($"User {userId} not found");

        // lazy loaded
        return user.UserInfo == null ? null : _mapper.Map<UserInfoModel>(user.UserInfo);
    }
}
