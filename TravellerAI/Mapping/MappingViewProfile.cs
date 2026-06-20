using AutoMapper;
using TravellerAI.Domain.Models;
using TravellerAI.Domain.ViewModels;

namespace TravellerAI.Mapping;

public class MappingViewProfile : Profile
{
    public MappingViewProfile()
    {
        CreateMap<UserModel, UserViewModel>().ReverseMap();
        CreateMap<TransportModel, TransportViewModel>()
            .ForMember(x => x.TotalBudget, z => z.MapFrom(o => o.Price));
            // ... add here conversion between Models and ViewModels
    }
}