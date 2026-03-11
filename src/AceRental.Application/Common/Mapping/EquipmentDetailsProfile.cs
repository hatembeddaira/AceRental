using AceRental.Application.Equipments.Dtos;
using AceRental.Application.Packs.Dtos;
using AceRental.Domain.Entities;
using AutoMapper;

namespace AceRental.Application.Common.Mapping
{
    public class EquipmentDetailsProfile : Profile
    {
        /// <inheritdoc/>
        public EquipmentDetailsProfile()
        {
            CreateMap<Equipment, EquipmentDetailsDto>()
                .ForMember(dest => dest.Reference, opt => opt.MapFrom(source => source.Reference))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(source => source.Description))
                .ForMember(dest => dest.DailyPrice, opt => opt.MapFrom(source => source.DailyPrice))
                .ForMember(dest => dest.PurchasePrice, opt => opt.MapFrom(source => source.PurchasePrice))
                .ForMember(dest => dest.NewPurchasePrice , opt => opt.MapFrom(source => source.NewPurchasePrice))
                .ForMember(dest => dest.TotalStock, opt => opt.MapFrom(source => source.TotalStock))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(source => source.Category))
                .ForMember(dest => dest.Packs, opt => opt.MapFrom(source => source.PackItems.Select(pi => new PackDto
                {
                    Id = pi.PackId,
                    Name = pi.Pack.Name,
                    Reference = pi.Pack.Reference
                })))
                .ReverseMap();
        }
    }
}