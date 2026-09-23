using AutoMapper;
using TravellerAI.Core.Features.AddBookingCommand;
using TravellerAI.Core.Features.Auth.RegisterUserCommand;
using TravellerAI.Core.Features.UpdateBookingCommand;
using TravellerAI.Core.Features.UpdateTripCommand;
using TravellerAI.Core.Features.UpdateUserProfileCommand;
using TravellerAI.Domain.Enums;
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

        CreateMap<UpdateTripCommand, TripModel>(MemberList.Source)
            // used for the ownership check only
            .ForSourceMember(c => c.UserId, o => o.DoNotValidate())
            // optional parts of the trip are changed only when they are sent
            .ForMember(m => m.Booking, o => o.Condition(c => c.Booking != null))
            .ForMember(m => m.Group, o => o.Condition(c => c.Group != null))
            .ForMember(m => m.Map, o => o.Condition(c => c.Map != null));

        CreateMap<AddBookingCommand, BookingModel>(MemberList.Source)
            .ForMember(m => m.CheckInDate, o => o.MapFrom(c => c.Period.Start))
            .ForMember(m => m.CheckOutDate, o => o.MapFrom(c => c.Period.End))
            .ForMember(m => m.Status, o => o.MapFrom(_ => BookingStatus.Pending))
            .ForSourceMember(c => c.TripId, o => o.DoNotValidate());

        CreateMap<UpdateBookingCommand, BookingModel>(MemberList.Source)
            .ForMember(m => m.CheckInDate, o => o.MapFrom(c => c.Period.Start))
            .ForMember(m => m.CheckOutDate, o => o.MapFrom(c => c.Period.End));
    }
}
