using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AceRental.Application.Clients.Dtos;
using AceRental.Domain.Entities;
using AutoMapper;

namespace AceRental.Application.Common.Mapping
{
    public class ClientReservationProfile : Profile
    {
        /// <inheritdoc/>
        public ClientReservationProfile()
        {
            CreateMap<Client, ClientReservationDto>()
                .ForMember(dest => dest.ClientNumber, opt => opt.MapFrom(source => source.ClientNumber))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(source => source.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(source => source.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(source => source.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(source => source.PhoneNumber))
                .ForMember(dest => dest.Address , opt => opt.MapFrom(source => source.Address))
                .ReverseMap();
        }
    }
}