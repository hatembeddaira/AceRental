using MediatR;
using Microsoft.EntityFrameworkCore;
using AceRental.Domain.Entities;
using AceRental.Infrastructure.Persistence;
using AceRental.Application.Services.Dtos;
using AutoMapper;
using AceRental.Domain.Enum;
using AceRental.Application.Exceptions;
using FluentValidation.Results;

namespace AceRental.Application.Services.Command;

public class CreateServiceHandler : IRequestHandler<CreateServiceCommand, ServiceDto>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateServiceHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ServiceDto> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Reference))
            throw new ValidationException(new List<ValidationFailure>
            {
                new ValidationFailure(nameof(request.Reference), "La référence du service est requise.")
            });
        if (string.IsNullOrEmpty(request.Name))
            throw new ValidationException(new List<ValidationFailure>
            {
                new ValidationFailure(nameof(request.Name), "Le nom du service est requis.")
            });
        if (request.DailyPriceHT <= 0)
            throw new ValidationException(new List<ValidationFailure>
            {
                new ValidationFailure(nameof(request.DailyPriceHT), "Le prix journalier est requis.")
            });
        
        var service = new Service
        {
            Name = request.Name,
            Reference = request.Reference,
            Type = request.Type,
            DailyPriceHT = request.DailyPriceHT
        };

        _context.Services.Add(service);
        await _context.SaveChangesAsync(cancellationToken);
        return _mapper.Map<ServiceDto>(service);
    }
}


