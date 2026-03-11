using AceRental.Application.Packs.Dtos;
using AceRental.Domain.Entities;
using AutoMapper;

namespace AceRental.Application.Common.Mapping
{
    public class PackDetailsProfile : Profile
    {
        /// <inheritdoc/>
        public PackDetailsProfile()
        {
            CreateMap<Pack, PackDetailsDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
                .ForMember(dest => dest.Reference, opt => opt.MapFrom(source => source.Reference))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(source => source.Description))
                .ForMember(dest => dest.DailyPrice, opt => opt.MapFrom(source => source.DailyPrice))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(source => source.Items))
                .ReverseMap();
        }
    }
}