using AceRental.Application.Services.Dtos;
using AceRental.Domain.Entities;
using AutoMapper;

namespace AceRental.Application.Common.Mapping
{
    public class ServiceProfile : Profile
    {
        /// <inheritdoc/>
        public ServiceProfile()
        {
            CreateMap<Service, ServiceDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
                .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(source => source.ServiceName))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(source => source.Type))
                .ForMember(dest => dest.DailyPriceHT, opt => opt.MapFrom(source => source.DailyPriceHT))
                .ReverseMap();
        }
    }
}
