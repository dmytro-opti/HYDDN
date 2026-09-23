using AutoMapper;
using TravellerAI.Core.Exceptions;
using TravellerAI.Core.Features.BuildJourneyCommand;
using TravellerAI.Core.Interfaces;
using TravellerAI.Core.Repositories;
using TravellerAI.Domain.Entities;
using TravellerAI.Domain.Entities.Owned;
using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Models;
using TravellerAI.Domain.ViewModels;

namespace TravellerAI.Core.Services;

public class JourneyService : IJourneyService
{
    private readonly IJourneyRepository _journeyRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILoggerService<JourneyService> _logger;
    private readonly IMapper _mapper;

    public JourneyService(IJourneyRepository journeyRepository, IUserRepository userRepository,
        ILoggerService<JourneyService> logger, IMapper mapper)
    {
        _journeyRepository = journeyRepository;
        _userRepository = userRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Guid> CreateJourney(BuildJourneyCommand command)
    {
        if (!await _userRepository.ExistsAsync(command.UserId))
        {
            throw new NotFoundException("User", command.UserId);
        }

        var journey = new JourneyEntity
        {
            UserId = command.UserId,
            Title = command.Title,
            Description = command.Description,
            Status = JourneyStatus.Active,
            Period = command.Period == null ? null : _mapper.Map<Period>(command.Period)
        };

        await _journeyRepository.AddAsync(journey);
        _logger.Log(ErrorLevel.Low, $"Journey {journey.Id} was created for user {command.UserId}");

        return journey.Id;
    }

    public async Task<JourneyModel> GetJourneyAsync(Guid journeyId)
    {
        var journey = await GetJourneyEntityAsync(journeyId);

        return _mapper.Map<JourneyModel>(journey);
    }

    public async Task<Guid> DeleteJourney(Guid journeyId)
    {
        if (!await _journeyRepository.DeleteAsync(journeyId))
        {
            throw new NotFoundException("Journey", journeyId);
        }

        _logger.Log(ErrorLevel.Low, $"Journey {journeyId} was deleted");

        return journeyId;
    }

    public async Task SelectPeriod(JourneyModel journey, PeriodViewModel period)
    {
        if (period == null || period.Start >= period.End)
        {
            throw new BadRequestException("Period start must be before period end");
        }

        var entity = await GetActiveJourneyEntityAsync(journey.Id);
        entity.Period = new Period { Start = period.Start, End = period.End };
        await _journeyRepository.UpdateAsync(entity);

        journey.Period = _mapper.Map<PeriodModel>(entity.Period);
    }

    public async Task SetMembers(JourneyModel journey, IEnumerable<string> members)
    {
        var entity = await GetActiveJourneyEntityAsync(journey.Id);

        entity.Members = (members ?? Enumerable.Empty<string>())
            .Where(m => !string.IsNullOrWhiteSpace(m))
            .Select(m => m.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        await _journeyRepository.UpdateAsync(entity);

        journey.Members = entity.Members;
    }

    public async Task<JourneyStatus> GetJourneyStatusAsync(Guid journeyId)
    {
        var journey = await GetJourneyEntityAsync(journeyId);

        return journey.Status;
    }

    private async Task<JourneyEntity> GetJourneyEntityAsync(Guid journeyId)
    {
        var journey = await _journeyRepository.GetByIdAsync(journeyId);

        if (journey == null)
        {
            _logger.Log(ErrorLevel.Medium, $"Journey {journeyId} not found");
            throw new NotFoundException("Journey", journeyId);
        }

        return journey;
    }

    /// <summary>
    /// Only active journeys can be changed.
    /// </summary>
    private async Task<JourneyEntity> GetActiveJourneyEntityAsync(Guid journeyId)
    {
        var journey = await GetJourneyEntityAsync(journeyId);

        if (journey.Status != JourneyStatus.Active)
        {
            throw new ConflictException($"Journey {journeyId} is {journey.Status} and cannot be changed");
        }

        return journey;
    }
}
