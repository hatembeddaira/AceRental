using AceRental.Application.Packs.Dtos;
using AceRental.Domain.Entities;
using AutoMapper;

namespace AceRental.Application.Common.Mapping
{
    public class PackProfile : Profile
    {
        /// <inheritdoc/>
        public PackProfile()
        {
            CreateMap<Pack, PackDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
                .ForMember(dest => dest.Reference, opt => opt.MapFrom(source => source.Reference))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(source => source.Description))
                .ForMember(dest => dest.DailyPriceHT, opt => opt.MapFrom(source => source.DailyPriceHT))
                .ReverseMap();
        }
    }
}