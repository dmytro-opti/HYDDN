using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Services;

public class LocationService : ILocationService
{
    public Task<LocationModel> SelectLocationAsync(Guid LocationId)
    {
        throw new NotImplementedException();
    }
}