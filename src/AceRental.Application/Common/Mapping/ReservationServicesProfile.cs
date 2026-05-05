using AceRental.Application.Reservations.Dtos;
using AceRental.Domain.Entities;
using AutoMapper;

namespace AceRental.Application.Common.Mapping
{
    public class ReservationServicesProfile : Profile
    {
        /// <inheritdoc/>
        public ReservationServicesProfile()
        {
            CreateMap<ReservationServices, ReservationServicesDto>()
                .ForMember(dest => dest.ReservationId, opt => opt.MapFrom(source => source.ReservationId))
                .ForMember(dest => dest.ServiceId, opt => opt.MapFrom(source => source.ServiceId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(source => source.Quantity))
                .ForMember(dest => dest.UnitPriceAtTimeOfBooking, opt => opt.MapFrom(source => source.UnitPriceAtTimeOfBooking))
                .ForMember(dest => dest.Reservation, opt => opt.MapFrom(source => source.Reservation))
                .ForMember(dest => dest.Service, opt => opt.MapFrom(source => source.Service))
                .ReverseMap();
        }
    }
}
