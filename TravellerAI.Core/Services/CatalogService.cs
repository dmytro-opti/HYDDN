using AutoMapper;
using TravellerAI.Core.Exceptions;
using TravellerAI.Core.Interfaces;
using TravellerAI.Core.Repositories;
using TravellerAI.Domain.Entities;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Services;

public class CatalogService : ICatalogService
{
    private readonly IRepository<CountryEntity> _countryRepository;
    private readonly IRepository<ActivityEntity> _activityRepository;
    private readonly IMapper _mapper;

    public CatalogService(IRepository<CountryEntity> countryRepository, IRepository<ActivityEntity> activityRepository, IMapper mapper)
    {
        _countryRepository = countryRepository;
        _activityRepository = activityRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<CountryModel>> GetCountriesAsync()
    {
        var countries = await _countryRepository.GetAllAsync();

        return _mapper.Map<List<CountryModel>>(countries.OrderBy(c => c.Name));
    }

    public async Task<IReadOnlyList<ActivityModel>> GetActivitiesAsync(Guid countryId, string? city)
    {
        if (!await _countryRepository.ExistsAsync(countryId))
        {
            throw new NotFoundException("Country", countryId);
        }

        city = string.IsNullOrWhiteSpace(city) ? null : city.Trim();

        // activities with an address can be trip stops
        var activities = await _activityRepository.FindAsync(a =>
            a.Location != null && a.Location.CountryId == countryId && (city == null || a.Location.City == city));

        return _mapper.Map<List<ActivityModel>>(activities.OrderByDescending(a => a.Rating));
    }
}
