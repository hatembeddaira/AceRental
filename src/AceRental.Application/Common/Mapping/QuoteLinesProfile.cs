using AceRental.Application.Quotes.Dtos;
using AceRental.Domain.Entities;
using AutoMapper;

namespace AceRental.Application.Common.Mapping
{
    public class QuoteLinesProfile : Profile
    {
        /// <inheritdoc/>
        public QuoteLinesProfile()
        {
            CreateMap<QuoteLines, QuoteLinesDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
                .ForMember(dest => dest.QuoteId, opt => opt.MapFrom(source => source.QuoteId))
                .ForMember(dest => dest.Reference, opt => opt.MapFrom(source => source.Reference))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.Name))
                .ForMember(dest => dest.DailyPriceHT, opt => opt.MapFrom(source => source.DailyPriceHT))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(source => source.Quantity))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(source => source.Type))
                .ForMember(dest => dest.Quote, opt => opt.MapFrom(source => source.Quote))
                .ReverseMap();
        }
    }
}
