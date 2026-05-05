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
        if (string.IsNullOrEmpty(request.ServiceName))
            throw new ValidationException(new List<ValidationFailure>
            {
                new ValidationFailure(nameof(request.ServiceName), "Le nom du service est requis.")
            });
        if (request.DailyPriceHT <= 0)
            throw new ValidationException(new List<ValidationFailure>
            {
                new ValidationFailure(nameof(request.DailyPriceHT), "Le prix journalier est requis.")
            });
        
        var Service = new ServiceDto
        {
            Id = Guid.NewGuid(),
            ServiceName = request.ServiceName,
            Type = request.Type,
            DailyPriceHT = request.DailyPriceHT
        };

        _context.Services.Add(_mapper.Map<Service>(Service));
        await _context.SaveChangesAsync(cancellationToken);
        return Service;
    }
}


