using AutoMapper;
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

    public Task<bool> UpdatePasswordAsync(Guid userId, string oldPassword, string newPassword)
    {
        throw new NotImplementedException();
    }

    public Task UpdateNameAsync(Guid userId, string firstName, string lastName)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateEmailAsync(Guid userId, string email)
    {
        var user = await GetUserAsync(userId);

        if (user.Email == email)
        {
            _logger.Log(ErrorLevel.Low, $"User {userId} has already been updated");
            return;
        }
        else
        {
            await _userRepository.UpdateEmailAsync(userId, email);
            _logger.Log(ErrorLevel.Low, $"User {userId} was updated successfully");
        }
    }

    public Task RemoveUserAsync(Guid userId)
    {
        throw new NotImplementedException();
    }
}