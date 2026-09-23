using System.Collections;
using System.Reflection;
using AutoMapper;
using TravellerAI.Domain.Entities;
using TravellerAI.Domain.Entities.Owned;
using TravellerAI.Domain.Models;

namespace TravellerAI.Mapping;

public class MappingEntitiesProfile : Profile
{
    public MappingEntitiesProfile()
    {
        CreateMap<Period, PeriodModel>().ReverseMap();

        // Entity -> Model
        CreateMap<UserEntity, UserModel>()
            // profile preferences are stored in UserInfo
            .ForMember(m => m.Interests, o => o.MapFrom(e => e.UserInfo!.Interests))
            .ForMember(m => m.TravelStyle, o => o.MapFrom(e => e.UserInfo!.TravelStyle))
            .ForMember(m => m.LookingFor, o => o.MapFrom(e => e.UserInfo!.LookingFor))
            .ForMember(m => m.Languages, o => o.MapFrom(e => e.UserInfo!.Languages))
            .ForMember(m => m.PersonalityType, o => o.MapFrom(e => e.UserInfo!.PersonalityType))
            .ForMember(m => m.Age, o => o.MapFrom(e => e.UserInfo != null ? e.UserInfo.Age : 0))
            .ForMember(m => m.ChoosenActivity, o => o.MapFrom(e => e.UserInfo!.ChoosenActivity))
            .ForMember(m => m.ChoosenTrip, o => o.MapFrom(e => e.UserInfo!.ChoosenTrip))
            .ForMember(m => m.MoneyAmount, o => o.MapFrom(e => e.UserInfo!.MoneyAmount));
        CreateMap<UserInfoEntity, UserInfoModel>()
            .ForMember(m => m.Destanation, o => o.MapFrom(e => e.Destination));
        CreateMap<JourneyEntity, JourneyModel>();
        CreateMap<TripEntity, TripModel>()
            .ForMember(m => m.TripId, o => o.MapFrom(e => e.Id));
        CreateMap<BookingEntity, BookingModel>()
            .ForMember(m => m.BookingId, o => o.MapFrom(e => e.Id))
            .ForMember(m => m.CreatedAt, o => o.MapFrom(e => e.Created))
            .ForMember(m => m.JourneyId, o => o.MapFrom(e => e.JourneyId ?? Guid.Empty))
            .ForMember(m => m.PropertyId, o => o.MapFrom(e => e.PropertyId ?? Guid.Empty))
            .ForMember(m => m.RoomId, o => o.MapFrom(e => e.RoomId ?? Guid.Empty));
        CreateMap<BudgetEntity, BudgetModel>();
        CreateMap<TransportEntity, TransportModel>();
        CreateMap<PlaceEntity, PlaceModel>();
        CreateMap<ReviewEntity, ReviewModel>()
            .ForMember(m => m.Coment, o => o.MapFrom(e => e.Comment))
            .ForMember(m => m.Titel, o => o.MapFrom(e => e.Title))
            .ForMember(m => m.PlaceID, o => o.MapFrom(e => e.PlaceId ?? Guid.Empty))
            .ForMember(m => m.CreatedAt, o => o.MapFrom(e => e.Created));
        CreateMap<ActivityEntity, ActivityModel>();
        CreateMap<LocationEntity, LocationModel>();

        // Model -> Entity (scalar values only)
        CreateMap<UserModel, UserEntity>().IgnoreEntityManagedMembers();
        CreateMap<UserInfoModel, UserInfoEntity>().IgnoreEntityManagedMembers()
            .ForMember(e => e.Destination, o => o.MapFrom(m => m.Destanation));
        CreateMap<JourneyModel, JourneyEntity>().IgnoreEntityManagedMembers();
        CreateMap<TripModel, TripEntity>().IgnoreEntityManagedMembers();
        CreateMap<BookingModel, BookingEntity>().IgnoreEntityManagedMembers()
            .ForMember(e => e.JourneyId, o => o.MapFrom(m => ToNullable(m.JourneyId)))
            .ForMember(e => e.PropertyId, o => o.MapFrom(m => ToNullable(m.PropertyId)))
            .ForMember(e => e.RoomId, o => o.MapFrom(m => ToNullable(m.RoomId)));
        CreateMap<BudgetModel, BudgetEntity>().IgnoreEntityManagedMembers();
        CreateMap<TransportModel, TransportEntity>().IgnoreEntityManagedMembers();
        CreateMap<PlaceModel, PlaceEntity>().IgnoreEntityManagedMembers();
        CreateMap<ReviewModel, ReviewEntity>().IgnoreEntityManagedMembers()
            .ForMember(e => e.Comment, o => o.MapFrom(m => m.Coment))
            .ForMember(e => e.Title, o => o.MapFrom(m => m.Titel))
            .ForMember(e => e.PlaceId, o => o.MapFrom(m => ToNullable(m.PlaceID)));
        CreateMap<ActivityModel, ActivityEntity>().IgnoreEntityManagedMembers();
        CreateMap<LocationModel, LocationEntity>().IgnoreEntityManagedMembers();
    }

    private static Guid? ToNullable(Guid id) => id == Guid.Empty ? null : id;
}

internal static class EntityMappingExpressionExtensions
{
    /// <summary>
    /// Ignores members EF Core is responsible for: Id, Created, Updated and navigation properties.
    /// Relations are set via foreign key properties, so mapping a model onto a tracked entity
    /// never creates duplicate related rows.
    /// </summary>
    public static IMappingExpression<TModel, TEntity> IgnoreEntityManagedMembers<TModel, TEntity>(
        this IMappingExpression<TModel, TEntity> expression) where TEntity : BaseEntity
    {
        expression.ForAllMembers(options =>
        {
            if (options.DestinationMember is PropertyInfo property && IsEntityManaged(property))
            {
                options.Ignore();
            }
        });

        return expression;
    }

    private static bool IsEntityManaged(PropertyInfo property)
    {
        if (property.Name is nameof(BaseEntity.Id) or nameof(BaseEntity.Created) or nameof(BaseEntity.Updated))
        {
            return true;
        }

        var type = property.PropertyType;
        if (typeof(BaseEntity).IsAssignableFrom(type))
        {
            return true;
        }

        return type != typeof(string)
               && typeof(IEnumerable).IsAssignableFrom(type)
               && type.IsGenericType
               && typeof(BaseEntity).IsAssignableFrom(type.GetGenericArguments()[0]);
    }
}
