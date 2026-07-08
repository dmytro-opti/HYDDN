using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Interfaces;

public interface ILocationService
{
    Task<LocationModel> SelectLocationAsync(Guid LocationId);
}