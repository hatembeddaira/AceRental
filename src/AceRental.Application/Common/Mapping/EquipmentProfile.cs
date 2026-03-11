using AceRental.Application.Equipments.Dtos;
using AceRental.Domain.Entities;
using AutoMapper;

namespace AceRental.Application.Common.Mapping
{
    public class EquipmentProfile : Profile
    {
        /// <inheritdoc/>
        public EquipmentProfile()
        {
            CreateMap<Equipment, EquipmentDto>()
                .ForMember(dest => dest.Reference, opt => opt.MapFrom(source => source.Reference))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(source => source.Description))
                .ForMember(dest => dest.DailyPrice, opt => opt.MapFrom(source => source.DailyPrice))
                .ForMember(dest => dest.PurchasePrice, opt => opt.MapFrom(source => source.PurchasePrice))
                .ForMember(dest => dest.NewPurchasePrice , opt => opt.MapFrom(source => source.NewPurchasePrice))
                .ForMember(dest => dest.TotalStock, opt => opt.MapFrom(source => source.TotalStock))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(source => source.Category))
                .ReverseMap();
        }
    }
}
