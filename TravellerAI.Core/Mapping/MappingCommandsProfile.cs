using AutoMapper;
using TravellerAI.Core.Features.Auth.RegisterUserCommand;
using TravellerAI.Core.Features.UpdateUserProfileCommand;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Mapping;

/// <summary>
/// Command -> Model mapping. Commands are mapped onto models loaded by services,
/// so members absent in a command keep their current values.
/// </summary>
public class MappingCommandsProfile : Profile
{
    public MappingCommandsProfile()
    {
        CreateMap<RegisterUserCommand, RegisterUserModel>();

        // relations are passed by ids
        CreateMap<Guid, ReferenceModel>().ConvertUsing(id => new ReferenceModel { Id = id });

        CreateMap<UpdateUserProfileCommand, UserModel>()
            .ForMember(m => m.Id, o => o.MapFrom(c => c.UserId))
            .ForMember(m => m.Name, o => o.MapFrom(c => c.Name))
            .ForMember(m => m.FirstName, o => o.MapFrom(c => c.FirstName))
            .ForMember(m => m.LastName, o => o.MapFrom(c => c.LastName))
            .ForMember(m => m.Profile, o => o.MapFrom(c => c))
            // email is changed by UpdateUserEmailCommand
            .ForMember(m => m.Email, o => o.Ignore())
            .ForMember(m => m.Journeys, o => o.Ignore());
        CreateMap<UpdateUserProfileCommand, UserInfoModel>()
            .ForMember(m => m.ChosenActivities, o => o.MapFrom(c => c.ChosenActivityIds))
            .ForMember(m => m.ChosenTrips, o => o.MapFrom(c => c.ChosenTripIds))
            .ForMember(m => m.PreferredCountries, o => o.MapFrom(c => c.PreferredCountryIds));

        CreateMap<Features.Trips.CreateTripCommand.CreateTripCommand, TripDraftModel>();
        CreateMap<Features.Trips.UpdateTripCommand.UpdateTripCommand, TripDraftModel>();
    }
}
