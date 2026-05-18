using AceRental.Application.Quotes.Dtos;
using AceRental.Domain.Entities;
using AutoMapper;

namespace AceRental.Application.Common.Mapping
{
    public class QuoteProfile : Profile
    {
        /// <inheritdoc/>
        public QuoteProfile()
        {
            CreateMap<Quote, QuoteDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
                .ForMember(dest => dest.QuoteNumber, opt => opt.MapFrom(source => source.QuoteNumber))
                .ForMember(dest => dest.IsArchived, opt => opt.MapFrom(source => source.IsArchived))
                .ForMember(dest => dest.ArchivedBy, opt => opt.MapFrom(source => source.ArchivedBy))
                .ForMember(dest => dest.ArchivedAt, opt => opt.MapFrom(source => source.ArchivedAt))
                .ForMember(dest => dest.ExpiryDate, opt => opt.MapFrom(source => source.ExpiryDate))
                .ForMember(dest => dest.TotalHT, opt => opt.MapFrom(source => source.TotalHT))
                .ForMember(dest => dest.TVA, opt => opt.MapFrom(source => source.TVA))
                .ForMember(dest => dest.ReservationId, opt => opt.MapFrom(source => source.ReservationId))
                .ForMember(dest => dest.Reservation, opt => opt.MapFrom(source => source.Reservation))
                .ForMember(dest => dest.QuoteLines, opt => opt.MapFrom(source => source.QuoteLines))
                .ReverseMap();
    }
}
}
