using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AceRental.Application.Reservations.Dtos;
using AceRental.Domain.Entities;
using AutoMapper;

namespace AceRental.Application.Common.Mapping
{
    public class ReservationHistoryProfile : Profile
    {
        /// <inheritdoc/>
        public ReservationHistoryProfile()
        {
            CreateMap<ReservationHistory, ReservationHistoryDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
                .ForMember(dest => dest.ReservationId, opt => opt.MapFrom(source => source.ReservationId))
                .ForMember(dest => dest.HistoryType, opt => opt.MapFrom(source => source.HistoryType))
                .ForMember(dest => dest.VersionNumber, opt => opt.MapFrom(source => source.VersionNumber))
                .ForMember(dest => dest.ChangeReason, opt => opt.MapFrom(source => source.ChangeReason))
                .ForMember(dest => dest.DataSnapshotJson, opt => opt.MapFrom(source => source.DataSnapshotJson))
                .ReverseMap();
        }
    }
}