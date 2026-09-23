using AutoMapper;
using TravellerAI.Core.Features.AddBookingCommand;
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
        CreateMap<UpdateUserProfileCommand, UserModel>(MemberList.Source)
            .ForMember(m => m.Id, o => o.MapFrom(c => c.UserId))
            .ForSourceMember(c => c.UserId, o => o.DoNotValidate());

        CreateMap<UpdateTripCommand, TripModel>(MemberList.Source)
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
