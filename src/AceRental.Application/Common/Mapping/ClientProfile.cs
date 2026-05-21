using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AceRental.Application.Clients.Dtos;
using AceRental.Domain.Entities;
using AutoMapper;

namespace AceRental.Application.Common.Mapping
{
    public class ClientProfile : Profile
    {
        /// <inheritdoc/>
        public ClientProfile()
        {
            CreateMap<Client, ClientDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
                .ForMember(dest => dest.ClientNumber, opt => opt.MapFrom(source => source.ClientNumber))
                .ForMember(dest => dest.RaisonSociale, opt => opt.MapFrom(source => source.RaisonSociale))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(source => source.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(source => source.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(source => source.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(source => source.PhoneNumber))
                .ForMember(dest => dest.TelNumber, opt => opt.MapFrom(source => source.TelNumber))
                .ForMember(dest => dest.Address , opt => opt.MapFrom(source => source.Address))
                .ForMember(dest => dest.ComplementAdresse, opt => opt.MapFrom(source => source.ComplementAdresse))
                .ForMember(dest => dest.City, opt => opt.MapFrom(source => source.City))
                .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(source => source.PostalCode))
                .ForMember(dest => dest.Reservations, opt => opt.MapFrom(source => source.Reservations))
                .ReverseMap();
        }
    }
}