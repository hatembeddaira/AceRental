using AceRental.Application.Reservations.Dtos;
using AceRental.Domain.Entities;
using AutoMapper;

namespace AceRental.Application.Common.Mapping
{
    public class ReservationItemProfile : Profile
    {
        /// <inheritdoc/>
        public ReservationItemProfile()
        {
            CreateMap<ReservationItem, ReservationItemDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
                .ForMember(dest => dest.ReservationId, opt => opt.MapFrom(source => source.ReservationId))
                .ForMember(dest => dest.Equipment, opt => opt.MapFrom(source => source.Equipment))
                .ForMember(dest => dest.PackId, opt => opt.MapFrom(source => source.PackId))
                .ForMember(dest => dest.Pack, opt => opt.MapFrom(source => source.Pack))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(source => source.Quantity))
                .ForMember(dest => dest.UnitPriceAtTimeOfBooking, opt => opt.MapFrom(source => source.UnitPriceAtTimeOfBooking))
                // .ForMember(dest => dest.Reservation, opt => opt.MapFrom(source => source.Reservation))
                .ReverseMap();
        }
    }
}
