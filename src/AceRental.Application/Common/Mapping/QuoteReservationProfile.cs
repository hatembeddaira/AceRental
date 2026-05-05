using AceRental.Application.Quotes.Dtos;
using AceRental.Domain.Entities;
using AutoMapper;

namespace AceRental.Application.Common.Mapping
{
    public class QuoteReservationProfile : Profile
    {
        /// <inheritdoc/>
        public QuoteReservationProfile()
        {
            CreateMap<Quote, QuoteReservationDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
                .ForMember(dest => dest.QuoteNumber, opt => opt.MapFrom(source => source.QuoteNumber))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(source => source.CreatedAt))
                .ForMember(dest => dest.ExpiryDate, opt => opt.MapFrom(source => source.ExpiryDate))
                .ForMember(dest => dest.TotalHT, opt => opt.MapFrom(source => source.TotalHT))
                .ForMember(dest => dest.TVA, opt => opt.MapFrom(source => source.TVA))
                .ForMember(dest => dest.TotalTTC, opt => opt.MapFrom(source => source.TotalTTC))
                .ForMember(dest => dest.ReservationId, opt => opt.MapFrom(source => source.ReservationId))
                .ReverseMap();
    }
}
}
