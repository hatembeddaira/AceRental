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
                .ForMember(dest => dest.DailyPriceHT, opt => opt.MapFrom(source => source.DailyPriceHT))
                .ForMember(dest => dest.PurchasePriceTTC, opt => opt.MapFrom(source => source.PurchasePriceTTC))
                .ForMember(dest => dest.NewPurchasePriceTTC , opt => opt.MapFrom(source => source.NewPurchasePriceTTC))
                .ForMember(dest => dest.TotalStock, opt => opt.MapFrom(source => source.TotalStock))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(source => source.Category))
                .ReverseMap();
        }
    }
}
