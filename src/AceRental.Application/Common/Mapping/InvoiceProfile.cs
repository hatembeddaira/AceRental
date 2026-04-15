using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AceRental.Application.Invoices.Dtos;
using AceRental.Domain.Entities;
using AutoMapper;

namespace AceRental.Application.Common.Mapping
{
    public class InvoiceProfile : Profile
    {
        /// <inheritdoc/>
        public InvoiceProfile()
        {
            CreateMap<Invoice, InvoiceDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
                .ForMember(dest => dest.InvoiceNumber, opt => opt.MapFrom(source => source.InvoiceNumber))
                .ForMember(dest => dest.AmountHT, opt => opt.MapFrom(source => source.AmountHT))
                .ForMember(dest => dest.IsPaid, opt => opt.MapFrom(source => source.IsPaid))
                .ForMember(dest => dest.ReservationId, opt => opt.MapFrom(source => source.ReservationId))
                .ReverseMap();
        }
    }
}
 