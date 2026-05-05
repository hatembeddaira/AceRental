using AceRental.Application.Reservations.Dtos;
using AceRental.Domain.Entities;
using AutoMapper;

namespace AceRental.Application.Common.Mapping
{
    public class ReservationDetailsProfile : Profile
    {
        /// <inheritdoc/>
        public ReservationDetailsProfile()
        {
            CreateMap<Reservation, ReservationDetailsDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
                .ForMember(dest => dest.ReservationNumber, opt => opt.MapFrom(source => source.ReservationNumber))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(source => source.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(source => source.EndDate))
                .ForMember(dest => dest.LogisticStatus, opt => opt.MapFrom(source => source.LogisticStatus))
                .ForMember(dest => dest.FinancialStatus, opt => opt.MapFrom(source => source.FinancialStatus))
                .ForMember(dest => dest.TotalHT, opt => opt.MapFrom(source => source.TotalHT))
                .ForMember(dest => dest.TVA, opt => opt.MapFrom(source => source.TVA))
                .ForMember(dest => dest.TotalTTC, opt => opt.MapFrom(source => source.TotalTTC))
                .ForMember(dest => dest.ClientId, opt => opt.MapFrom(source => source.ClientId))
                .ForMember(dest => dest.Client, opt => opt.MapFrom(source => source.Client))
                .ForMember(dest => dest.Equipments, opt => opt.MapFrom(source => source.Equipments))
                .ForMember(dest => dest.Packs, opt => opt.MapFrom(source => source.Packs))
                .ForMember(dest => dest.Services, opt => opt.MapFrom(source => source.Services))
                .ForMember(dest => dest.Invoices, opt => opt.MapFrom(source => source.Invoices))
                .ForMember(dest => dest.Quotes, opt => opt.MapFrom(source => source.Quotes))
                .ForMember(dest => dest.Payments, opt => opt.MapFrom(source => source.Payments))
                .ReverseMap();
        }
    }
}
