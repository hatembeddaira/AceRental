using AceRental.Application.Packs.Dtos;
using AceRental.Domain.Entities;
using AutoMapper;

namespace AceRental.Application.Common.Mapping
{
    public class PackItemProfile : Profile
    {
        /// <inheritdoc/>
        public PackItemProfile()
        {
            CreateMap<PackItem, PackItemDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
                .ForMember(dest => dest.PackId, opt => opt.MapFrom(source => source.PackId))
                .ForMember(dest => dest.EquipmentId, opt => opt.MapFrom(source => source.EquipmentId))
                .ForMember(dest => dest.Equipment, opt => opt.MapFrom(source => source.Equipment))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(source => source.Quantity))
                .ReverseMap();
        }
    }
}