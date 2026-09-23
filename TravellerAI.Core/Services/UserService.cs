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
    private readonly IRepository<ActivityEntity> _activityRepository;
    private readonly ITripRepository _tripRepository;
    private readonly IRepository<CountryEntity> _countryRepository;
    private readonly IIdentityService _identityService;
    private readonly IValidationService _validationService;
    private readonly ILoggerService<UserService> _logger;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IRepository<ActivityEntity> activityRepository,
        ITripRepository tripRepository, IRepository<CountryEntity> countryRepository, IIdentityService identityService,
        IValidationService validationService, ILoggerService<UserService> logger, IMapper mapper)
    {
        _userRepository = userRepository;
        _activityRepository = activityRepository;
        _tripRepository = tripRepository;
        _countryRepository = countryRepository;
        _identityService = identityService;
        _validationService = validationService;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<UserModel> GetUserAsync(Guid userId)
    {
        var user = await GetUserEntityAsync(userId);

        return _mapper.Map<UserModel>(user);
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

        // identity login email and profile email are changed together
        await _identityService.ChangeEmailAsync(userId, email);
        _logger.Log(ErrorLevel.Low, $"User {userId} email was updated successfully");
    }

    public async Task RemoveUserAsync(Guid userId)
    {
        await _identityService.DeleteUserAsync(userId);
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
        var profile = user.Profile ?? throw new BadRequestException("Profile is not specified");

        if (!await _validationService.ValidateBirthDate(profile))
        {
            throw new BadRequestException(
                $"Age must be between {Constants.Validation.MinUserAge} and {Constants.Validation.MaxUserAge} years");
        }

        var entity = await GetUserEntityAsync(user.Id);
        entity.Name = user.Name;
        entity.FirstName = user.FirstName;
        entity.LastName = user.LastName;

        // profile preferences are stored in UserInfo, created on first profile update
        var info = entity.UserInfo ??= new UserInfoEntity();
        info.BirthDate = profile.BirthDate?.Date;
        info.TravelStyle = profile.TravelStyle;
        info.PersonalityType = profile.PersonalityType;
        info.BudgetLevel = profile.BudgetLevel;
        info.CompanionGender = profile.CompanionGender;
        info.LookingFor = string.IsNullOrWhiteSpace(profile.LookingFor) ? null : profile.LookingFor.Trim();
        info.Languages = profile.Languages.Select(l => l.Trim().ToLowerInvariant()).Distinct().ToList();
        info.Interests = profile.Interests.Distinct().ToList();

        Replace(info.ChosenActivities, await LoadAsync(_activityRepository, profile.ChosenActivities, "Activities"));
        Replace(info.ChosenTrips, await LoadAsync(_tripRepository, profile.ChosenTrips, "Trips"));
        Replace(info.PreferredCountries, await LoadAsync(_countryRepository, profile.PreferredCountries, "Countries"));

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

    /// <summary>
    /// Loads related entities by ids; all ids must exist.
    /// </summary>
    private static async Task<IReadOnlyList<TEntity>> LoadAsync<TEntity>(IRepository<TEntity> repository,
        IEnumerable<ReferenceModel> references, string name) where TEntity : BaseEntity
    {
        var ids = references.Select(r => r.Id).Distinct().ToList();
        if (ids.Count == 0)
        {
            return Array.Empty<TEntity>();
        }

        var entities = await repository.FindAsync(e => ids.Contains(e.Id));

        var missing = ids.Except(entities.Select(e => e.Id)).ToList();
        if (missing.Count > 0)
        {
            throw new NotFoundException($"{name} not found: {string.Join(", ", missing)}");
        }

        return entities;
    }

    private static void Replace<TEntity>(ICollection<TEntity> collection, IEnumerable<TEntity> items)
    {
        collection.Clear();
        foreach (var item in items)
        {
            collection.Add(item);
        }
    }
}
