using AutoMapper;
using TravellerAI.Domain.Entities;
using TravellerAI.Domain.Models;

namespace TravellerAI.Mapping;

public class MappingEntitiesProfile : Profile
{
    public MappingEntitiesProfile()
    {
        CreateMap<UserModel, UserEntity>().ReverseMap();
            // ... add here conversion between Models and Entities
    }
}