using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AceRental.Application.Payments.Dtos;
using AceRental.Domain.Entities;
using AutoMapper;

namespace AceRental.Application.Common.Mapping
{
    public class PaymentDetailsProfile : Profile
    {
        /// <inheritdoc/>
        public PaymentDetailsProfile()
        {
            CreateMap<Payment, PaymentDetailsDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
                .ForMember(dest => dest.ReservationId, opt => opt.MapFrom(source => source.ReservationId))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(source => source.Amount))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(source => source.Date))
                .ForMember(dest => dest.Method, opt => opt.MapFrom(source => source.Method))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(source => source.Type))
                .ForMember(dest => dest.TransactionId, opt => opt.MapFrom(source => source.TransactionId))
                .ForMember(dest => dest.Reservation, opt => opt.MapFrom(source => source.Reservation))
                .ReverseMap();
        }
    }
}