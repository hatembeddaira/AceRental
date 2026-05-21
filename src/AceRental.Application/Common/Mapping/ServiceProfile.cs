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
                .ForMember(dest => dest.Reference, opt => opt.MapFrom(source => source.Reference))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.Name))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(source => source.Type))
                .ForMember(dest => dest.PriceHT, opt => opt.MapFrom(source => source.PriceHT))
                .ForMember(dest => dest.IsDailyPrice, opt => opt.MapFrom(source => source.IsDailyPrice))
                .ReverseMap();
        }
    }
}
