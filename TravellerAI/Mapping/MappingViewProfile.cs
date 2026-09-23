using AutoMapper;
using TravellerAI.Core.Features.AddBookingCommand;
using TravellerAI.Core.Features.AddTransportCommand;
using TravellerAI.Core.Features.BuildJourneyCommand;
using TravellerAI.Core.Features.BuildTripCommand;
using TravellerAI.Core.Features.GetAvailableLocationCommand;
using TravellerAI.Core.Features.SelectBookingCommand;
using TravellerAI.Core.Features.UpdateBookingCommand;
using TravellerAI.Core.Features.UpdateTripCommand;
using TravellerAI.Core.Features.UpdateUserProfileCommand;
using TravellerAI.Core.Features.User.UpdateUserEmailCommand;
using TravellerAI.Domain.Models;
using TravellerAI.Domain.ViewModels;
using TravellerAI.Domain.ViewModels.Requests;

namespace TravellerAI.Mapping;

/// <summary>
/// Controller level mapping: request ViewModel -> Command, Model -> response ViewModel.
/// Ids from the route are set by controllers after mapping.
/// </summary>
public class MappingViewProfile : Profile
{
    public MappingViewProfile()
    {
        CreateMap<PeriodModel, PeriodViewModel>().ReverseMap();

        // Model -> ViewModel
        CreateMap<UserModel, UserViewModel>();
        CreateMap<UserInfoModel, UserProfileViewModel>()
            .ForMember(v => v.Destination, o => o.MapFrom(m => m.Destanation));
        CreateMap<BudgetModel, BudgetViewModel>();
        CreateMap<BookingModel, BookingViewModel>()
            .ForMember(v => v.JourneyId, o => o.MapFrom(m => ToNullable(m.JourneyId)))
            .ForMember(v => v.PropertyId, o => o.MapFrom(m => ToNullable(m.PropertyId)))
            .ForMember(v => v.RoomId, o => o.MapFrom(m => ToNullable(m.RoomId)))
            .ForMember(v => v.TotalBudget, o => o.Ignore());
        CreateMap<TransportModel, TransportViewModel>()
            .ForMember(x => x.TotalBudget, z => z.MapFrom(o => o.Price));
        // UserId, JourneyId, JourneyTitle are flattened from User / Journey
        CreateMap<TripModel, TripViewModel>();
        CreateMap<LocationModel, LocationViewModel>();

        // Request -> Command
        CreateMap<CreateJourneyRequest, BuildJourneyCommand>(MemberList.Source);
        CreateMap<BuildTripRequest, BuildTripCommand>(MemberList.Source);
        CreateMap<UpdateTripRequest, UpdateTripCommand>(MemberList.Source);
        CreateMap<TripBookingRequest, BookingModel>(MemberList.Source)
            .ForMember(m => m.BookingId, o => o.MapFrom(r => r.BookingId ?? Guid.Empty))
            .ForMember(m => m.PropertyId, o => o.MapFrom(r => r.PropertyId ?? Guid.Empty))
            .ForMember(m => m.RoomId, o => o.MapFrom(r => r.RoomId ?? Guid.Empty))
            .ForMember(m => m.CheckInDate, o => o.MapFrom(r => r.Period.Start))
            .ForMember(m => m.CheckOutDate, o => o.MapFrom(r => r.Period.End))
            .ForMember(m => m.Currency, o => o.MapFrom(r => r.Currency ?? "USD"));
        CreateMap<AddTransportRequest, AddTransportCommand>(MemberList.Source);
        CreateMap<AddBookingRequest, AddBookingCommand>(MemberList.Source);
        CreateMap<UpdateBookingRequest, UpdateBookingCommand>(MemberList.Source);
        CreateMap<SelectBookingRequest, SelectBookingCommand>(MemberList.Source);
        CreateMap<UpdateUserProfileRequest, UpdateUserProfileCommand>(MemberList.Source);
        CreateMap<UpdateUserEmailRequest, UpdateUserEmailCommand>(MemberList.Source);
        CreateMap<LocationSearchRequest, GetAvailableLocationListCommand>(MemberList.Source);
    }

    private static Guid? ToNullable(Guid id) => id == Guid.Empty ? null : id;
}
