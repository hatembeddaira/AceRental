using MediatR;
using Microsoft.EntityFrameworkCore;
using AceRental.Domain.Entities;
using AceRental.Infrastructure.Persistence;
using AceRental.Application.Exceptions;
using FluentValidation.Results;
using AutoMapper;
using AceRental.Domain.Enum;

namespace AceRental.Application.Services.Command;

public class UpdateServiceHandler : IRequestHandler<UpdateServiceCommand, bool>
{
    private readonly ApplicationDbContext _context;

    public UpdateServiceHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
    {
        if (request.DailyPriceHT <= 0)
            throw new ValidationException(new List<ValidationFailure>
            {
                new ValidationFailure(nameof(request.DailyPriceHT), "Le prix journalier est requis.")
            });

        var Service = await _context.Services.FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);
        if (Service == null)
            throw new NotFoundException(nameof(Service), request.Id);

        if (!string.IsNullOrEmpty(request.ServiceName))
            Service.ServiceName = request.ServiceName;

        if (request.Type != null)
            Service.Type = (ServiceType)request.Type;

        if (request.DailyPriceHT != null)
        Service.DailyPriceHT = (decimal)request.DailyPriceHT;
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}


