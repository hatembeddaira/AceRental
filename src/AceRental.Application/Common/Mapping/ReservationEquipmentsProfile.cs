using AceRental.Application.Reservations.Dtos;
using AceRental.Domain.Entities;
using AutoMapper;

namespace AceRental.Application.Common.Mapping
{
    public class ReservationEquipmentsProfile : Profile
    {
        /// <inheritdoc/>
        public ReservationEquipmentsProfile()
        {
            CreateMap<ReservationEquipments, ReservationEquipmentsDto>()
                .ForMember(dest => dest.ReservationId, opt => opt.MapFrom(source => source.ReservationId))
                .ForMember(dest => dest.EquipmentId, opt => opt.MapFrom(source => source.EquipmentId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(source => source.Quantity))
                .ForMember(dest => dest.UnitPriceAtTimeOfBooking, opt => opt.MapFrom(source => source.UnitPriceAtTimeOfBooking))
                .ForMember(dest => dest.Reservation, opt => opt.MapFrom(source => source.Reservation))
                .ForMember(dest => dest.Equipment, opt => opt.MapFrom(source => source.Equipment))
                .ReverseMap();
        }
    }
}
