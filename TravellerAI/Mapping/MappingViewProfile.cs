using AutoMapper;
using TravellerAI.Core.Features.Auth.ChangePasswordCommand;
using TravellerAI.Core.Features.Auth.LoginUserCommand;
using TravellerAI.Core.Features.Auth.RefreshTokenCommand;
using TravellerAI.Core.Features.Auth.RegisterUserCommand;
using TravellerAI.Core.Features.GetAvailableLocationCommand;
using TravellerAI.Core.Features.Journeys.AddJourneyHotelCommand;
using TravellerAI.Core.Features.Journeys.AddJourneyTransportCommand;
using TravellerAI.Core.Features.Journeys.CreateJourneyCommand;
using TravellerAI.Core.Features.Journeys.SearchJourneyHotelsCommand;
using TravellerAI.Core.Features.Journeys.UpdateJourneyHotelCommand;
using TravellerAI.Core.Features.Transports.SearchTransportsCommand;
using TravellerAI.Core.Features.Trips.CreateTripCommand;
using TravellerAI.Core.Features.Trips.SearchTripsCommand;
using TravellerAI.Core.Features.Trips.UpdateTripCommand;
using TravellerAI.Core.Features.UpdateUserProfileCommand;
using TravellerAI.Core.Features.User.UpdateUserEmailCommand;
using TravellerAI.Domain.Models;
using TravellerAI.Domain.ViewModels;
using TravellerAI.Domain.ViewModels.Requests;
using TravellerAI.Domain.ViewModels.Responses;

namespace TravellerAI.Mapping;

/// <summary>
/// Controller level mapping: request ViewModel -> Command, Model -> response ViewModel.
/// Ids from the route and the current user id are set by controllers after mapping.
/// </summary>
public class MappingViewProfile : Profile
{
    public MappingViewProfile()
    {
        CreateMap<PeriodModel, PeriodViewModel>().ReverseMap();

        // Model -> ViewModel
        CreateMap<UserModel, UserViewModel>();
        CreateMap<UserInfoModel, UserProfileViewModel>();
        CreateMap<ReferenceModel, ReferenceViewModel>();
        CreateMap<AuthUserModel, AuthUserViewModel>();
        CreateMap<AuthResultModel, AuthResponse>()
            .ForMember(v => v.AccessToken, o => o.MapFrom(m => m.AccessToken.Token))
            .ForMember(v => v.AccessTokenExpiresAt, o => o.MapFrom(m => m.AccessToken.ExpiresAt))
            .ForMember(v => v.RefreshToken, o => o.MapFrom(m => m.RefreshToken.Token))
            .ForMember(v => v.RefreshTokenExpiresAt, o => o.MapFrom(m => m.RefreshToken.ExpiresAt));
        CreateMap<BudgetModel, BudgetViewModel>();
        CreateMap<BookingModel, BookingViewModel>()
            .ForMember(v => v.JourneyId, o => o.MapFrom(m => ToNullable(m.JourneyId)))
            .ForMember(v => v.PropertyId, o => o.MapFrom(m => ToNullable(m.PropertyId)))
            .ForMember(v => v.RoomId, o => o.MapFrom(m => ToNullable(m.RoomId)));
        CreateMap<TransportModel, TransportViewModel>();
        CreateMap<LocationModel, LocationViewModel>();
        CreateMap<CountryModel, CountryViewModel>();
        CreateMap<ActivityModel, ActivityViewModel>();
        CreateMap<NotificationModel, NotificationViewModel>();
        CreateMap<JourneyModel, JourneyViewModel>();
        CreateMap<JourneyModel, JourneySummaryViewModel>();
        CreateMap<JourneyDayModel, JourneyDayViewModel>();
        CreateMap<JourneyProgressModel, JourneyProgressViewModel>();
        CreateMap<JourneyStepModel, JourneyStepViewModel>();
        // stop location is flattened (LocationName, LocationLatitude, ...)
        CreateMap<TripModel, TripViewModel>();
        CreateMap<TripStopModel, TripStopViewModel>();
        CreateMap<HotelOfferModel, HotelOfferViewModel>();

        // Request -> Command
        CreateMap<RegisterRequest, RegisterUserCommand>(MemberList.Source);
        CreateMap<LoginRequest, LoginUserCommand>(MemberList.Source);
        CreateMap<RefreshTokenRequest, RefreshTokenCommand>(MemberList.Source);
        CreateMap<ChangePasswordRequest, ChangePasswordCommand>(MemberList.Source);
        CreateMap<UpdateUserProfileRequest, UpdateUserProfileCommand>(MemberList.Source);
        CreateMap<UpdateUserEmailRequest, UpdateUserEmailCommand>(MemberList.Source);
        CreateMap<LocationSearchRequest, GetAvailableLocationListCommand>(MemberList.Source);
        CreateMap<CreateJourneyRequest, CreateJourneyCommand>(MemberList.Source);
        CreateMap<HotelSearchRequest, SearchJourneyHotelsCommand>(MemberList.Source);
        CreateMap<JourneyHotelRequest, AddJourneyHotelCommand>(MemberList.Source);
        CreateMap<UpdateJourneyHotelRequest, UpdateJourneyHotelCommand>(MemberList.Source);
        CreateMap<AddTransportRequest, AddJourneyTransportCommand>(MemberList.Source);
        CreateMap<TripStopRequest, TripStopDraftModel>(MemberList.Source);
        CreateMap<TripRequest, CreateTripCommand>(MemberList.Source);
        CreateMap<TripRequest, UpdateTripCommand>(MemberList.Source);
        CreateMap<TripSearchRequest, SearchTripsCommand>(MemberList.Source);
        CreateMap<TransportSearchRequest, SearchTransportsCommand>(MemberList.Source);
    }

    private static Guid? ToNullable(Guid id) => id == Guid.Empty ? null : id;
}
